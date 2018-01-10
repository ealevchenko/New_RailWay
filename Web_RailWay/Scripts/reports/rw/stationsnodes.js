//$(document).ready(function () {
$( function () {
    var lang = $.cookie('lang');

    var tabs,
        tabs_active = 0,
        edit_dialog,
        form_edit,
        delete_confirm,
        form_delete
        id_station_select = 1,      // станция по которой идет выборка
        id_select_send = null,      // выбранная строка отпрвки.
        id_select_arrival = null,   // выбранная строка прибытия.
        id_nodes = $('input[name ="id-nodes"]'),
        nodes = $('input[name ="nodes"]'),
        station_from_label = $('label[name = "station-from"]'),
        station_from_select = $('select[name = "station-from"]'),
        side_station_from = $('select[name ="side-station-from"]'),
        transfer_type = $('select[name ="transfer-type"]'),
        station_on_select = $('select[name ="station-on"]'),
        station_on_label = $('label[name ="station-on"]'),
        side_station_on = $('select[name ="side-station-on"]'),
        allFields = $([]).add(id_nodes).add(nodes).add(station_from_label).add(station_from_select).add(side_station_from).add(transfer_type).add(station_on_select).add(station_on_label).add(side_station_on),
        delete_confirm_text = $('label[name = "delete-confirm-text"]'),
        tips = $(".validateTips");

    function updateTips(t) {
        tips
          .text(t)
          .addClass("ui-state-highlight");
        setTimeout(function () {
            tips.removeClass("ui-state-highlight", 1500);
        }, 500);
    }

    function checkLength(o, n, min, max) {
        if (o.val().length > max || o.val().length < min) {
            o.addClass("ui-state-error");
            updateTips("Длина названия узла должна быть от " + min + " до " + max + ".");
            return false;
        } else {
            return true;
        }
    }

    function checkRegexp(o, regexp, n) {
        if (!(regexp.test(o.val()))) {
            o.addClass("ui-state-error");
            updateTips(n);
            return false;
        } else {
            return true;
        }
    }

    // Показать кнопки редактирования
    function showButtonEdit() {
        $('button[name ="bt-send-add"]').show();
        $('button[name ="bt-arrival-add"]').show();
        if (id_select_send != null) {
            $('button[name ="bt-send-edit"]').show();
            $('button[name ="bt-send-delete"]').show();
        } else {
            $('button[name ="bt-send-edit"]').hide();
            $('button[name ="bt-send-delete"]').hide();
        }
        if (id_select_arrival != null) {
            $('button[name ="bt-arrival-edit"]').show();
            $('button[name ="bt-arrival-delete"]').show();
        } else {
            $('button[name ="bt-arrival-edit"]').hide();
            $('button[name ="bt-arrival-delete"]').hide();
        }
    }

    //Показать информацию по узлам
    function ViewStationNodes(id) {
        OnBegin();
            if (id_station_select != id) {
                id_select_send = null;
                id_select_arrival = null;
        }
        id_station_select = id;
        getTbodySendStationsNodes(id, $('tbody[name ="list-send-stations"]'), id_select_send)
        getTbodyArrivalStationsNodes(id, $('tbody[name ="list-arrival-stations"]'), id_select_arrival)
        LockScreenOff();
        // определим видимость кнопок
        showButtonEdit();
        // определим события выбор поля
        $('tr[name="send-station"]').click(function (evt) {
            evt.preventDefault();
            $('tr[name="send-station"]').removeClass('selected');
            $(this).addClass('selected');
            id_select_send = $(this).attr("id");
            // Показать кнопки
            showButtonEdit();
        });

        $('tr[name="arrival-station"]').click(function (evt) {
            evt.preventDefault();
            $('tr[name="arrival-station"]').removeClass('selected');
            $(this).addClass('selected');
            id_select_arrival = $(this).attr("id");
            // Показать кнопки
            showButtonEdit();
        });
    }

    function EditNode(id) {
        // Получить узел
        if (id == 0) {
            // Добавить
                    id_nodes.val(id);
                    nodes.val();
                    if (tabs_active == 0) {
                        station_from_label.show();
                        station_from_label.text(getStationName(id_station_select));
                        selectListStations(station_on_select, 0, null, getListIDStationsOfSendStationsNodes(id_station_select, null))
                        station_on_select.selectmenu('enable');
                        station_on_label.hide();
                        // спрятать station_from_select
                        station_from_select.selectmenu();
                        station_from_select.selectmenu("destroy");
                        station_from_select.hide();
                    } else {
                        selectListStations(station_from_select, 0, null, getListIDStationsOfArrivalStationsNodes(id_station_select, null))
                        station_from_select.selectmenu('enable');
                        station_on_label.show();
                        station_on_label.text(getStationName(id_station_select));
                        station_from_label.hide();
                        // спрятать station_on_select
                        station_on_select.selectmenu();
                        station_on_select.selectmenu('destroy');
                        station_on_select.hide();
                    }
                    selectListSideStation(side_station_from, 0, null);
                    selectListTransferType(transfer_type, 0, null);

                    selectListSideStation(side_station_on, 0, null);
                    edit_dialog.dialog("option", "title", "Добавить узел" + (tabs_active == 0 ? " отправки." : " прибытия."));
                    edit_dialog.dialog("open");
        } else {
            $.ajax({
                url: '/railway/api/rw/stations_nodes/id/' + id,
                type: 'GET',
                dataType: 'json',
                success: function (jsondata) {
                    id_nodes.val(id);
                    nodes.val(jsondata.nodes);
                    if (tabs_active == 0) {
                        station_from_label.show();
                        station_from_label.text(getStationName(jsondata.id_station_from));
                        selectListStations(station_on_select, jsondata.id_station_on, null, getListIDStationsOfSendStationsNodes(jsondata.id_station_from, jsondata.id_station_on))
                        station_on_select.selectmenu('enable');
                        station_on_label.hide();
                        // спрятать station_from_select
                        station_from_select.selectmenu();
                        station_from_select.selectmenu("destroy");
                        station_from_select.hide();
                    } else {
                        selectListStations(station_from_select, jsondata.id_station_from, null, getListIDStationsOfArrivalStationsNodes(jsondata.id_station_on, jsondata.id_station_from))
                        station_from_select.selectmenu('enable');
                        station_on_label.show();
                        station_on_label.text(getStationName(jsondata.id_station_on));
                        station_from_label.hide();
                        // спрятать station_on_select
                        station_on_select.selectmenu();
                        station_on_select.selectmenu('destroy');
                        station_on_select.hide();
                    }
                    selectListSideStation(side_station_from, jsondata.side_station_from, null);
                    selectListTransferType(transfer_type, jsondata.transfer_type, null);
                    selectListSideStation(side_station_on, jsondata.side_station_on, null);
                    edit_dialog.dialog("option", "title", "Править узел " + (tabs_active == 0 ? " отправки." : " прибытия."));
                    edit_dialog.dialog("open");
                },
                error: function (x, y, z) {
                    LockScreenOff();
                    alert(x + '\n' + y + '\n' + z);
                }
            });
        }
    }

    function Edit() {
        var valid = true;
        allFields.removeClass("ui-state-error");
        valid = valid && checkLength(nodes, "nodes", 1, 50);
        //valid = valid && checkRegexp(nodes, /^[a-z]([0-9a-z_\s])+$/i, "Username may consist of a-z, 0-9, underscores, spaces and must begin with a letter.");
        valid = valid && checkRegexp(nodes, /^[a-zA-Zа-яА-Я0-9\-\+\>\<\.\,\*\(\)\ ]+$/i, "Название узла может состоять из букв a-Z, а-Я, цифр 0-9, и символов -+<>(),.*");
        if (valid) {
            var new_station_node = {
                id: Number(id_nodes.val()),
                nodes: nodes[0].value,
                id_station_from: tabs_active == 0 ? Number(id_station_select) : Number(station_from_select.val()),
                side_station_from: Boolean(Number(side_station_from.val())),
                id_station_on: tabs_active == 0 ?  Number(station_on_select.val()) : Number(id_station_select),
                side_station_on: Boolean(Number(side_station_on.val())),
                transfer_type: Number(transfer_type.val())
            };
            edit_dialog.dialog("close");

            $.ajax({
                url: '/railway/api/rw/stations_nodes/' + new_station_node.id,
                type: 'PUT',
                data: JSON.stringify(new_station_node),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    ViewStationNodes(id_station_select)
                },
                error: function (x, y, z) {
                    alert(x + '\n' + y + '\n' + z);
                }
            });
        }
        return valid;
    }

    function DeleteNode(id) {
        $.ajax({
            url: '/railway/api/rw/stations_nodes/id/' + id,
            type: 'GET',
            dataType: 'json',
            success: function (jsondata) {
                id_nodes.val(id);
                delete_confirm_text.text("Вы хотите удалить узел [" + "(" + id + ")" + jsondata.nodes + "," + getStationName(jsondata.id_station_from) +"]?");
                delete_confirm.dialog("option", "title", "Удалить узел");
                delete_confirm.dialog("open");
            },
            error: function (x, y, z) {
                LockScreenOff();
                alert(x + '\n' + y + '\n' + z);
            }
        });

    }

    function Delete() {
        $.ajax({
            url: '/railway/api/rw/stations_nodes/' + Number(id_nodes.val()),
            type: 'DELETE',
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                if (tabs_active == 0) {
                    id_select_send = null;
                } else {
                    id_select_arrival = null;
                }
                ViewStationNodes(id_station_select)
            },
            error: function (x, y, z) {
                alert(x + '\n' + y + '\n' + z);
            }
        });
        delete_confirm.dialog("close");
    }

    // панель править узел
    edit_dialog = $('#edit-panel').dialog({
        resizable: false,
        modal: true,
        autoOpen: false,
        height: "auto",
        width: 500,
        buttons: {
            "Сохранить": Edit,
            Cancel: function () {
                edit_dialog.dialog("close");
            }
        },
        close: function () {
            form_edit[0].reset();
            allFields.removeClass("ui-state-error");
        }
    });

    form_edit = edit_dialog.find("form#edit").on("submit", function (event) {
        event.preventDefault();
        Edit();
    });

    // панель править узел
    delete_confirm = $('#delete-confirm').dialog({
        resizable: false,
        modal: true,
        autoOpen: false,
        height: "auto",
        width: 500,
        buttons: {
            "Удалить": Delete,
            Cancel: function () {
                delete_confirm.dialog("close");
            }
        },
        close: function () {
            form_delete[0].reset();
        }
    });

    form_delete = delete_confirm.find("form#delete").on("submit", function (event) {
        event.preventDefault();
        Delete();
    });
    // Спрячем кнопки
    $('button[name ="bt-send-add"]').hide();
    $('button[name ="bt-send-edit"]').hide();
    $('button[name ="bt-send-delete"]').hide();
    $('button[name ="bt-arrival-add"]').hide();
    $('button[name ="bt-arrival-edit"]').hide();
    $('button[name ="bt-arrival-delete"]').hide();
    // Событие нажатия кн
    $('button[name ="bt-send-edit"]').click(function (evt) {
        evt.preventDefault();
        EditNode(id_select_send);
    });
    $('button[name ="bt-arrival-edit"]').click(function (evt) {
        evt.preventDefault();
        EditNode(id_select_arrival);
    });
    // Событие нажатия кн
    $('button[name ="bt-send-add"]').click(function (evt) {
        evt.preventDefault();
        EditNode(0);
    });
    $('button[name ="bt-arrival-add"]').click(function (evt) {
        evt.preventDefault();
        EditNode(0);
    });
    // Событие нажатия кн
    $('button[name ="bt-send-delete"]').click(function (evt) {
        evt.preventDefault();
        DeleteNode(id_select_send);
    });
    $('button[name ="bt-arrival-delete"]').click(function (evt) {
        evt.preventDefault();
        DeleteNode(id_select_arrival);
    });
    // Настройка списка станций
    selectListStations(
        $('select[name ="station"]'),
        id_station_select,
        function (event, ui) {
            event.preventDefault();
            var id = ui.item.value;
            ViewStationNodes(id);
        },
        null);

    //табс
    tabs = $("#tabs-stations").tabs({
        activate: function (event, ui) {
            tabs_active = tabs.tabs("option", "active");
        }
    });

    // Таблица send
    $('#send-table').DataTable({
        "paging": false,
        "ordering": false,
        "info": false,
        language: {
            decimal: lang == 'en' ? "." : ",",
            search: lang == 'en' ? "Search" : "Найти",
        },
        jQueryUI: true,
    }).draw();

    $('#arrival-table').DataTable({
        "paging": false,
        "ordering": false,
        "info": false,
        language: {
            decimal: lang == 'en' ? "." : ",",
            search: lang == 'en' ? "Search" : "Найти",
        },
        jQueryUI: true,
    }).draw();

    // Покажем 
    ViewStationNodes(id_station_select)
});

function OnBegin() {
    lang = $.cookie('lang');
    LockScreen(lang == 'en' ? 'We are processing your request ...' : 'Мы обрабатываем ваш запрос...');
}

function OnFailure(request, error) {
    //alert("This is the OnFailure Callback:" + error);
    LockScreenOff();
    alert("Ошибка: " + error);
}

function OnComplete(request, status) {
    //alert("This is the OnComplete Callback: " + status);   
    LockScreenOff();
}



$(function () {

    // Список общесистемных слов 
    $.Text_View =
        {
            'default':  //default language: ru
            {
                'text_link_tabs_cars_list': 'Браки в работе',
                'text_link_tabs_cars_user': 'Отчеты',
                'field_id': 'id',
                'field_Number': 'Номер карты',
                'field_DriverName': 'Driver Name',
                'field_AutoNumber': 'Номер транспортного средства',
                'field_Debitor': 'Код дебитора',
                'field_Sn1': 'Код трансп. средства',
                'field_Sn2': 'Код трансп. средства 2',
                'field_AutoModel': 'Модель трансп. средства',
                'field_Street': 'Автоколонна',
                'field_House': 'Шифр цеха',
                'field_Owner': 'Правил',
                'field_Active': 'Активация',
                'title_insert_cards': 'Добавить карту',
                'title_edit_cards': 'Править карту',
                'title_delete_cards': 'Удалить карту',
                'button_insert': 'Добавить',
                'button_edit': 'Править',
                'button_delete': 'Удалить',
                'legend_delete_text': 'Вы действительно хотите удалить эту RFID-карту?',

            },
            'en':  //default language: English
            {
                'text_link_tabs_cars_list': 'RFID-Cards',
                'text_link_tabs_cars_user': 'Users',
                'field_id': 'id',
                'field_Number': 'Card number',
                'field_DriverName': 'Driver Name',
                'field_AutoNumber': 'Vehicle Number',
                'field_Debitor': 'Customer Code',
                'field_Sn1': 'Vehicle code',
                'field_Sn2': 'Vehicle code 2',
                'field_AutoModel': 'Vehicle Model',
                'field_Street': 'motorcade',
                'field_House': 'Shop Code',
                'field_Owner': 'Rules',
                'field_Active': 'Activation',
                'title_insert_cards': 'Add a card',
                'title_edit_cards': 'Edit card',
                'title_delete_cards': 'Delete Card',
                'button_insert': 'Add',
                'button_edit': 'Edit',
                'button_delete': 'Delete',
                'legend_delete_text': 'Do you really want to delete this RFID card?',
            }
        };

    var lang = $.cookie('lang'),
        langs = $.extend(true, $.extend(true, getLanguages($.Text_View, lang), getLanguages($.Text_Common, lang)), getLanguages($.Text_Table, lang)),
        //function_delete_cards_b = Boolean(Number(function_delete_cards)),
        // Загрузка библиотек
        //loadReference = function (callback) {
        //    LockScreen(langView('mess_load', langs));
        //    var count = 1;
        //    // Загрузка списка цехов (common.js)
        //    getReference_azsDeparts(function (result) {
        //        reference_departs = result;
        //        count -= 1;
        //        if (count <= 0) {
        //            if (typeof callback === 'function') {
        //                LockScreenOff();
        //                callback();
        //            }
        //        }
        //    })
        //},
        //// список цехов
        //reference_departs = null,
        // Типы отчетов
        tab_type_cards = {
            html_div: $("div#tabs-reports"),
            active: 0,
            initObject: function () {
                $('#link-tab-report-1').text(langView('text_link_tabs_cars_list', langs));
                $('#link-tab-report-2').text(langView('text_link_tabs_cars_user', langs));
                this.html_div.tabs({
                    collapsible: true,
                    activate: function (event, ui) {
                        tab_type_cards.active = tab_type_cards.html_div.tabs("option", "active");
                        //tab_type_cards.activeTable(tab_type_cards.active, false);
                    },
                });
                //this.activeTable(this.active, true);
            },
            activeTable: function (active, data_refresh) {
                if (active == 0) {
                    table_cards.viewTable(data_refresh);
                }
                //if (active == 1) {
                //    table_sending.viewTable(data_refresh);
                //}

            },

        },
        // Панель таблицы
        panel_table_cards = {
            html_div_panel: $('<div class="setup-operation" id="property"></div>'),
            button_insert: $('<button class="ui-button ui-widget ui-corner-all"></button>'),
            button_edit: $('<button class="ui-button ui-widget ui-corner-all"></button>'),
            button_delete: $('<button class="ui-button ui-widget ui-corner-all"></button>'),
            initPanel: function (obj) {
                this.html_div_panel
                    .append(this.button_insert.text(langView('button_insert', langs)))
                    .append(this.button_edit.text(langView('button_edit', langs)).hide())
                //if (function_delete_cards_b) {
                    this.html_div_panel.append(this.button_delete.text(langView('button_delete', langs)).hide())
                //}
                obj.prepend(this.html_div_panel);
                // Обработка события закрыть детали
                this.button_insert.on('click', function () {
                    confirm_ins_edit_panel.Open(null);
                });
                this.button_edit.on('click', function () {

                    confirm_ins_edit_panel.Open(table_cards.select_id);
                });
                this.button_delete.on('click', function () {
                    confirm_delete_panel.Open(table_cards.select_id);
                });
            },
        },
        // Таблица RFID-карт
        table_cards = {
            html_table: $('table#table-report-1'),
            obj_table: null,
            select: null,
            select_id: null,
            list: [],
            // Инициализировать таблицу
            initObject: function () {
                this.obj = this.html_table.DataTable({
                    "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                    "paging": true,
                    "ordering": true,
                    "info": false,
                    "select": false,
                    //"filter": true,
                    //"scrollY": "600px",
                    //"scrollX": true,
                    language: language_table(langs),
                    jQueryUI: true,
                    "createdRow": function (row, data, index) {
                        $(row).attr('id', data.id);
                        table_cards.viewUpdateActive(row, data.Active)
                    },
                    columns: [
                        { data: "id", title: langView('field_id', langs), width: "50px", orderable: true, searchable: false },
                        { data: "Number", title: langView('field_Number', langs), width: "100px", orderable: true, searchable: true },
                        { data: "AutoNumber", title: langView('field_AutoNumber', langs), width: "100px", orderable: true, searchable: true },
                        { data: "Debitor", title: langView('field_Debitor', langs), width: "100px", orderable: true, searchable: true },
                        { data: "Sn1", title: langView('field_Sn1', langs), width: "100px", orderable: true, searchable: false },
                        { data: "AutoModel", title: langView('field_AutoModel', langs), width: "100px", orderable: true, searchable: true },
                        { data: "Street", title: langView('field_Street', langs), width: "100px", orderable: true, searchable: false },
                        { data: "HouseName", title: langView('field_House', langs), width: "100px", orderable: true, searchable: true },
                        { data: "Active", title: langView('field_Active', langs), width: "50px", orderable: true, searchable: false },
                    ],
                });
                this.initEventSelect();
                panel_table_cards.initPanel($('DIV#table-panel'));
            },
            // Показать таблицу с данными
            viewTable: function (data_refresh) {
                LockScreen(langView('mess_delay', langs));
                if (this.list == null | data_refresh == true) {
                    // Обновим данные
                    getAsyncViewazsCards(
                        function (result) {
                            table_cards.list = result;
                            table_cards.loadDataTable(result);
                            table_cards.obj.draw();
                        }
                    );
                } else {
                    table_cards.loadDataTable(this.list);
                    table_cards.obj.draw();
                };
            },
            // Загрузить данные
            loadDataTable: function (data) {
                this.list = data;
                table_cards.obj.clear();
                for (i = 0; i < data.length; i++) {
                    this.obj.row.add({
                        "id": data[i].Id,
                        "Number": data[i].Number,
                        "DriverName": data[i].DriverName,
                        "AutoNumber": data[i].AutoNumber,
                        "Debitor": data[i].Debitor,
                        "Sn1": data[i].Sn1,
                        "Sn2": data[i].Sn2,
                        "AutoModel": data[i].AutoModel,
                        "Street": data[i].Street,
                        "House": data[i].House,
                        "CreateDate": data[i].CreateDate,
                        "CreateTime": data[i].CreateTime,
                        "UpdateTime": data[i].UpdateTime,
                        "Owner": data[i].Owner,
                        "UpdateDate": data[i].UpdateDate,
                        "Active": data[i].Active,
                        "HouseName": reference_departs != null ? reference_departs.getText(data[i].House) : data[i].House,
                    });
                }
                LockScreenOff();
            },
            // Инициализация события выбора поля
            initEventSelect: function () {
                this.html_table.find('tbody')
                    .on('click', 'tr', function () {
                        table_cards.select_id = $(this).attr('id');
                        var select = getObjects(table_cards.list, 'Id', table_cards.select_id);
                        if (select != null && select.length > 0) {
                            table_cards.select = select[0];
                        };
                        table_cards.clearSelect();
                        $(this).addClass('selected');
                        panel_table_cards.button_edit.show();
                        panel_table_cards.button_delete.show();
                    });
            },
            // Сбросить выбор поля
            clearSelect: function () {
                this.html_table.find('tbody tr').removeClass('selected');
            },
            // Обновим
            viewUpdateString: function (data) {
                if (data != null) {
                    var row_ind = table_cards.obj.row('#' + data.Id).index();
                    table_cards.obj.cell(row_ind, 1).data(data.Number);
                    table_cards.obj.cell(row_ind, 2).data(data.AutoNumber);
                    table_cards.obj.cell(row_ind, 3).data(data.Debitor);
                    table_cards.obj.cell(row_ind, 4).data(data.Sn1);
                    table_cards.obj.cell(row_ind, 5).data(data.AutoModel);
                    table_cards.obj.cell(row_ind, 6).data(data.Street);
                    table_cards.obj.cell(row_ind, 7).data(reference_departs != null ? reference_departs.getText(data.House) : data.House);
                    table_cards.viewUpdateActive(table_cards.html_table.find('tbody tr.selected'), data.Active)
                    table_cards.obj.draw(false);
                }
            },
            // добавить
            viewInsertString: function (data) {
                if (data != null) {
                    this.obj.row.add({
                        "id": data.Id,
                        "Number": data.Number,
                        "DriverName": data.DriverName,
                        "AutoNumber": data.AutoNumber,
                        "Debitor": data.Debitor,
                        "Sn1": data.Sn1,
                        "Sn2": data.Sn2,
                        "AutoModel": data.AutoModel,
                        "Street": data.Street,
                        "House": data.House,
                        "CreateDate": data.CreateDate,
                        "CreateTime": data.CreateTime,
                        "UpdateTime": data.UpdateTime,
                        "Owner": data.Owner,
                        "UpdateDate": data.UpdateDate,
                        "Active": data.Active,
                        "HouseName": reference_departs != null ? reference_departs.getText(data.House) : data.House,
                    });
                    table_cards.obj.draw();
                }
            },
            // удалить
            viewDeleteString: function (data) {
                if (data != null) {
                    table_cards.obj.row('.selected').remove().draw(false);
                }
            },
            // Показать поле активности
            viewUpdateActive: function (row, active) {
                if (active == null) {
                    $('td', row).eq(8).html("").removeClass().addClass('null_active');
                } else {
                    if (active) { $('td', row).eq(8).html("").removeClass().addClass('active'); }
                    else { $('td', row).eq(8).html("").removeClass().addClass('not_active'); }
                }
            },

        },
        // Панель добавить обновить карту
        confirm_ins_edit_panel = {
            html_div: $("#ins-edit-confirm"),
            id: 0,
            create_date: null,
            create_time: null,
            user: null,
            initObject: function () {
                this.obj = this.html_div.dialog({
                    resizable: false,
                    modal: true,
                    autoOpen: false,
                    height: "auto",
                    width: 600,
                    buttons: {
                        Save: function () {
                            $(this).dialog("close");
                            var cards = confirm_ins_edit_panel.getNewCards();
                            if (cards.Id == 0) {
                                // Добавить
                                postAsyncazsCards(cards,
                                    function (result) {
                                        // Проверка на ошибку
                                        if (result > 0) {
                                            // ошибки нет
                                            getAsyncViewazsCardsOfID(result, function (result) {
                                                // Обновим
                                                table_cards.viewInsertString(result)
                                            });
                                        } else {
                                            alert('error insert')
                                        }
                                    });
                            } else {
                                // Обновить
                                putAsyncazsCards(cards,
                                    function (result) {
                                        // Проверка на ошибку
                                        if (result > 0) {
                                            // ошибки нет
                                            getAsyncViewazsCardsOfID(confirm_ins_edit_panel.id, function (result) {
                                                // Обновим
                                                table_cards.viewUpdateString(result)
                                            });
                                        } else {
                                            alert('error update')
                                        }
                                    });
                            }
                        },
                        Cancel: function () {
                            $(this).dialog("close");
                        }
                    }
                });
                $('#label-number-cards').text(langView('field_Number', langs) + ':');
                $('#label-drivername-cards').text(langView('field_DriverName', langs) + ':');
                $('#label-autonumber-cards').text(langView('field_AutoNumber', langs) + ':');
                $('#label-debitor-cards').text(langView('field_Debitor', langs) + ':');
                $('#label-sn1-cards').text(langView('field_Sn1', langs) + ':');
                $('#label-sn2-cards').text(langView('field_Sn2', langs) + ':');
                $('#label-automodel-cards').text(langView('field_AutoModel', langs) + ':');
                $('#label-street-cards').text(langView('field_Street', langs) + ':');
                $('#label-house-cards').text(langView('field_House', langs) + ':');
                $('#label-owner-cards').text(langView('field_Owner', langs) + ':');
                $('#label-active-cards').text(langView('field_Active', langs) + ':');
            },
            Open: function (id) {
                if (id != null) {
                    getAsyncViewazsCardsOfID(id,
                        function (result) {
                            confirm_ins_edit_panel.setCards(result);
                            confirm_ins_edit_panel.obj.dialog("option", "title", id == null ? langView('title_insert_cards', langs) : langView('title_edit_cards', langs));
                            confirm_ins_edit_panel.obj.dialog("open");
                        });
                } else {
                    confirm_ins_edit_panel.setCards(null);
                    confirm_ins_edit_panel.obj.dialog("option", "title", id == null ? langView('title_insert_cards', langs) : langView('title_edit_cards', langs));
                    confirm_ins_edit_panel.obj.dialog("open");
                }

            },
            setCards: function (cards) {
                confirm_ins_edit_panel.user = $('input#username').val();
                if (cards != null) {
                    // режим edit
                    confirm_ins_edit_panel.id = cards.Id;
                    confirm_ins_edit_panel.create_date = cards.CreateDate;
                    confirm_ins_edit_panel.create_time = cards.CreateTime;
                    $('#Number').val(cards.Number);
                    $('#DriverName').val(cards.DriverName);
                    $('#AutoNumber').val(cards.AutoNumber);
                    $('#Debitor').val(cards.Debitor);
                    $('#Sn1').val(cards.Sn1);
                    $('#Sn2').val(cards.Sn2);
                    $('#AutoModel').val(cards.AutoModel);
                    $('#Street').val(cards.Street);
                    $('#Owner').val($('input#username').val());
                    $('#Active').prop('checked', cards.Active);
                } else {
                    // режим insert
                    confirm_ins_edit_panel.resetCards()
                }
                var sel = $('select[name ="House"]');
                initSelect(
                    sel,
                    { width: 300 },
                    reference_departs.list,
                    function (row) {
                        return { value: Number(row.id), text: row.name };
                    },
                    cards != null ? cards.House : -1,
                    function (event, ui) {
                        event.preventDefault();

                    },
                    null);
            },
            // Сбросить
            resetCards: function () {
                confirm_ins_edit_panel.id = 0;
                confirm_ins_edit_panel.create_date = null;
                confirm_ins_edit_panel.create_time = null;
                $('#Number').val('');
                $('#DriverName').val('');
                $('#AutoNumber').val('');
                $('#Debitor').val('');
                $('#Sn1').val('');
                $('#Sn2').val('');
                $('#AutoModel').val('');
                $('#Street').val('');
                $('#Owner').val($('input#username').val());
                $('#Active').prop('checked', false);
            },
            // Получить cards после правки
            getNewCards: function () {
                var now = new Date();
                return cards = {
                    Id: Number(confirm_ins_edit_panel.id),
                    Number: $('#Number').val(),
                    DriverName: $('#DriverName').val() != '' ? $('#DriverName').val() : null,
                    AutoNumber: $('#AutoNumber').val(),
                    Debitor: $('#Debitor').val() != '' ? Number($('#Debitor').val()) : null,
                    Sn1: $('#Sn1').val() != '' ? $('#Sn1').val() : null,
                    Sn2: $('#Sn2').val() != '' ? $('#Sn2').val() : null,
                    AutoModel: $('#AutoModel').val() != '' ? $('#AutoModel').val() : null,
                    Street: $('#Street').val() != '' ? Number($('#Street').val()) : null,
                    House: $('#House').val() != '' ? Number($('#House').val()) : null,
                    CreateDate: confirm_ins_edit_panel.id == 0 ? toISOStringTZ(now).substring(0, 10) : confirm_ins_edit_panel.create_date,
                    CreateTime: confirm_ins_edit_panel.id == 0 ? toISOStringTZ(now).substring(11, 23) : confirm_ins_edit_panel.create_time,
                    UpdateDate: toISOStringTZ(now).substring(0, 10),
                    UpdateTime: toISOStringTZ(now).substring(11, 23),
                    Owner: confirm_ins_edit_panel.user != null ? confirm_ins_edit_panel.user : null,
                    Active: $('#Active').prop('checked'),
                };
            },
        },
        // Панель добавить обновить карту
        confirm_delete_panel = {
            html_div: $("#delete-confirm"),
            id: 0,
            initObject: function () {
                this.obj = this.html_div.dialog({
                    resizable: false,
                    modal: true,
                    autoOpen: false,
                    height: "auto",
                    width: 600,
                    buttons: {
                        "Удалить": function () {
                            $(this).dialog("close");
                            deleteAsynczsCards(confirm_delete_panel.id,
                                function (result) {
                                    // Проверка на ошибку
                                    if (result > 0) {
                                        // ошибки нет
                                        table_cards.viewDeleteString(confirm_delete_panel.id)

                                    } else {
                                        alert('error delete')
                                    }
                                });
                        },
                        Cancel: function () {
                            $(this).dialog("close");
                        }
                    }
                });
                $('#legend-delete').text(langView('legend_delete_text', langs));
                $('#label-id-cards').text(langView('field_id', langs) + ':');
                $('#label-number-cards').text(langView('field_Number', langs) + ':');
                $('#label-drivername-cards').text(langView('field_DriverName', langs) + ':');
                $('#label-autonumber-cards').text(langView('field_AutoNumber', langs) + ':');
                $('#label-debitor-cards').text(langView('field_Debitor', langs) + ':');
                $('#label-sn1-cards').text(langView('field_Sn1', langs) + ':');
                $('#label-sn2-cards').text(langView('field_Sn2', langs) + ':');
                $('#label-automodel-cards').text(langView('field_AutoModel', langs) + ':');
                $('#label-street-cards').text(langView('field_Street', langs) + ':');
                $('#label-house-cards').text(langView('field_House', langs) + ':');
                $('#label-owner-cards').text(langView('field_Owner', langs) + ':');
                $('#label-active-cards').text(langView('field_Active', langs) + ':');
            },
            Open: function (id) {
                if (id != null) {
                    getAsyncViewazsCardsOfID(id,
                        function (result) {
                            confirm_delete_panel.setCards(result);
                            confirm_delete_panel.obj.dialog("option", "title", langView('title_delete_cards', langs));
                            confirm_delete_panel.obj.dialog("open");
                        });
                }
            },
            setCards: function (cards) {
                if (cards != null) {
                    confirm_delete_panel.id = cards.Id;
                    $('#value-id-cards').text(cards.Id);
                    $('#value-number-cards').text(cards.Number);
                    $('#value-drivername-cards').text(cards.DriverName);
                    $('#value-autonumber-cards').text(cards.AutoNumber);
                    $('#value-debitor-cards').text(cards.Debitor);
                    $('#value-sn1-cards').text(cards.Sn1);
                    $('#value-sn2-cards').text(cards.Sn2);
                    $('#value-automodel-cards').text(cards.AutoModel);
                    $('#value-street-cards').text(cards.Street);
                    $('#value-house-cards').text(reference_departs != null ? reference_departs.getText(cards.House) : cards.House);
                    $('#value-owner-cards').text(cards.Owner);
                    $('#value-active-cards').text(cards.Active);
                }
            },
        }
    //-----------------------------------------------------------------------------------------
    // Функции
    //-----------------------------------------------------------------------------------------

    //-----------------------------------------------------------------------------------------
    // Инициализация объектов
    //-----------------------------------------------------------------------------------------
    confirm_ins_edit_panel.initObject();
    confirm_delete_panel.initObject();
    tab_type_cards.initObject();
    // Загрузка библиотек
    //loadReference(function (result) {
        table_cards.initObject();
        //tab_type_cards.activeTable(tab_type_cards.active, true);
    //});

});
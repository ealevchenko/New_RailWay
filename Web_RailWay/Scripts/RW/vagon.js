$(function () {
    //-----------------------------------------------------------------------------------------
    // Объявление глобальных переменных
    //-----------------------------------------------------------------------------------------
    allVars = $.getUrlVars();   // Получить параметры get запроса
    var lang = $.cookie('lang'),
        resurses = {
            list: null,
            initObject: function (file, callback) {
                initLang(file, function (json) {
                    resurses.list = json;
                    if (typeof callback === 'function') {
                        callback(json);
                    }
                })
            },
            getText: function (tag) {
                var result = null;
                var str = getObjects(resurses.list, 'tag', tag);
                if (str != null) {
                    result = lang == 'en' ? str[0].en : str[0].ru;
                }
                return result;
            }
        },
        svagon = $("#vagon").spinner({
            spin: function (event, ui) {
                if (ui.value > 99999999) {
                    $(this).spinner("value", 1);
                    return false;
                } else if (ui.value < 1) {
                    $(this).spinner("value", 99999999);
                    return false;
                }
            }
        }),
        bt_searsh = $('#search').button({
            icon: "ui-icon-search"
        }).on('click', function (evt) {
            evt.preventDefault();
            table_history_car.viewTable(true);
            }),
        //
        rw_reference_country = {
            list: [],
            initObject: function () {
                getAsyncReferenceCountry(function (result)
                { rw_reference_country.list = result; });
            },
            getCountry: function (id) {
                var country = getObjects(this.list, 'id', id)
                if (country != null && country.length > 0) {
                    return country[0];
                }
            },
        },
        //
        rw_reference_cargo = {
            list: [],
            initObject: function () {
                getAsyncReferenceCargo(function (result)
                { rw_reference_cargo.list = result; });
            },
            getCargo: function (id) {
                var cargo = getObjects(this.list, 'id', id)
                if (cargo != null && cargo.length > 0) {
                    return cargo[0];
                }
            },
        },
        //
        rw_reference_consignee = {
            list: [],
            initObject: function () {
                getAsyncReferenceConsignee(function (result)
                { rw_reference_consignee.list = result; });
            },
            getConsignee: function (id) {
                var consignee = getObjects(this.list, 'id', id)
                if (consignee != null && consignee.length > 0) {
                    return consignee[0];
                }
            },
        }, 
        //
        rw_reference_station = {
            list: [],
            initObject: function () {
                getAsyncReferenceStation(function (result)
                { rw_reference_station.list = result; });
            },
            getStation: function (id) {
                var station = getObjects(this.list, 'id', id)
                if (station != null && station.length > 0) {
                    return station[0];
                }
            },
        }, 
        // Тупики
        rw_deadlock = {
            list: [],
            initObject: function () {
                getAsyncDeadlock(function (result)
                { rw_deadlock.list = result; });
            },
            getDeadlock: function (id) {
                var deadlock = getObjects(this.list, 'id', id)
                if (deadlock != null && deadlock.length > 0) {
                    return deadlock[0];
                }
            },
        }, 

        // Типы отчетов
        tab_type_vagons = {
            html_div: $("#tabs-report-vagons"),
            active: 0,
            initObject: function () {
                $('#link-tabs-history').text(resurses.getText("link_tabs_history"));
                $('#link-tabs-info').text(resurses.getText("link_tabs_info"));
                this.html_div.tabs({
                    collapsible: true,
                    activate: function (event, ui) {
                        tab_type_vagons.active = tab_type_vagons.html_div.tabs("option", "active");
                        //tab_type_vagons.activeTable(tab_type_vagons.active, false);
                    },
                });
                //this.activeTable(this.active, true);
            },
            activeTable: function (active, data_refresh) {
                //if (active == 0) {
                //    table_arrival.viewTable(data_refresh);
                //}
                //if (active == 1) {
                //    table_sending.viewTable(data_refresh);
                //}
                //if (active == 2) {
                //    //table_wt_routes.viewTable(table_wt_routes.view, data_refresh);
                //}
                //if (active == 3) {
                //    //table_wt_last.viewTable(data_refresh);
                //}
            }

        },
        //
        accordion_history = {
            html_history_car: $("#accordion-history-car"),
            html_history_sap: $("#accordion-history-sap"),
            html_history_arrival: $("#accordion-history-arrival"),
            icons: {
                header: "ui-icon-circle-arrow-e",
                activeHeader: "ui-icon-circle-arrow-s"
            },
            initObject: function () {
                this.html_history_car.accordion({
                    icons: this.icons,
                    heightStyle: "content",
                    collapsible: true,
                    activate: function (event, ui) {
                        var active = accordion_history.html_history_car.accordion("option", "active");
                        if (active === 0) {
                            //table_vagon.viewTableVagon(false);
                        }
                    }
                });
                this.html_history_sap.accordion({
                    icons: this.icons,
                    heightStyle: "content",
                    collapsible: true,
                    activate: function (event, ui) {
                        var active = accordion_history.html_history_car.accordion("option", "active");
                        if (active === 0) {
                            //table_vagon.viewTableVagon(false);
                        }
                    }
                });
                this.html_history_arrival.accordion({
                    icons: this.icons,
                    heightStyle: "content",
                    collapsible: true,
                    activate: function (event, ui) {
                        var active = accordion_history.html_history_arrival.accordion("option", "active");
                        if (active === 0) {
                            //table_vagon.viewTableNatHist(false);
                        }
                    }
                });
            }
        },
        //table-list-history-car
        table_history_car = {
            html_table: $('#table-list-history-car'),
            obj_table: null,
            obj: null,
            list: [],
            // Инициализировать таблицу
            initObject: function () {
                this.obj = this.html_table.DataTable({
                    "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                    "paging": true,
                    "ordering": true,
                    "info": false,
                    //"select": false,
                    //"filter": true,
                    //"scrollY": "600px",
                    //"scrollX": true,
                    language: {
                        emptyTable: resurses.getText("table_message_emptyTable"),
                        decimal: resurses.getText("table_decimal"),
                        search: resurses.getText("table_message_search"),
                    },
                    jQueryUI: true,
                    "createdRow": function (row, data, index) {
                        $(row).attr('id', data.id);
                        var bt_delete = $('<button id=' + data.id + ' class="ui-button ui-widget ui-corner-all" name="close"><span class="ui-icon ui-icon-closethick"></span>' + (lang == 'en' ? 'Delete' : 'Удалить') + '</button>');
                        var bt_correct = $('<button id=' + data.id + ' class="ui-button ui-widget ui-corner-all" name="close"><span class="ui-icon ui-icon-pencil"></span>' + (lang == 'en' ? 'Correction' : 'Коррекция') + '</button>');

                        $('td', row).eq(16).append(bt_delete).append(bt_correct);
                        bt_delete.on('click', function (evt) {
                            evt.preventDefault();
                            var id = $(this).attr("id")
                            confirm_delete_panel.Open(id);
                        });

                    },
                    columns: [
                        {
                            className: 'details-control',
                            orderable: false,
                            data: null,
                            defaultContent: '',
                            searchable: false, width: "30px"
                        },
                        { data: "position", title: resurses.getText("table_field_position"), width: "50px", orderable: true, searchable: false },
                        { data: "num", title: resurses.getText("table_field_num"), width: "50px", orderable: false, searchable: false },
                        { data: "id_sostav", title: resurses.getText("table_field_id_sostav"), width: "50px", orderable: false, searchable: true },
                        { data: "id_arrival", title: resurses.getText("table_field_id_arrival"), width: "50px", orderable: false, searchable: true },
                        { data: "dt_uz", title: resurses.getText("table_field_dt_uz"), width: "150px", orderable: true, searchable: true },
                        { data: "dt_inp_amkr", title: resurses.getText("table_field_dt_inp_amkr"), width: "150px", orderable: true, searchable: true },
                        { data: "natur_kis", title: resurses.getText("table_field_natur_kis"), width: "50px", orderable: false, searchable: true },
                        { data: "dt_out_amkr", title: resurses.getText("table_field_dt_out_amkr"), width: "150px", orderable: true, searchable: true },
                        { data: "natur_kis_out", title: resurses.getText("table_field_natur_kis_out"), width: "50px", orderable: true, searchable: true },
                        { data: "natur", title: resurses.getText("table_field_natur"), width: "50px", orderable: false, searchable: true },
                        { data: "dt_create", title: resurses.getText("table_field_dt_create"), width: "150px", orderable: true, searchable: true },
                        { data: "user_create", title: resurses.getText("table_field_user_create"), width: "150px", orderable: false, searchable: true },
                        { data: "dt_close", title: resurses.getText("table_field_dt_close"), width: "150px", orderable: true, searchable: true },
                        { data: "user_close", title: resurses.getText("table_field_user_close"), width: "150px", orderable: false, searchable: true },
                        { data: "parent_id", title: resurses.getText("table_field_parent_id"), width: "50px", orderable: false, searchable: true },
                        { data: null, defaultContent: '', width: "50px", orderable: false, searchable: false },
                    ],
                });
                this.obj_table = $('DIV#table-list-history-car_wrapper');
                //panel_table_arrival.initPanel(this.obj_table);
                this.initEventSelect();
                this.obj.order([1, 'desc']);
                //this.initEventSelectChild();
                //this.obj.columns([10, 11, 12]).visible(false, true);
            },
            // Показать таблицу с данными
            viewTable: function (data_refresh) {
                OnBegin();
                if (this.list == null | data_refresh == true) {
                    // Обновим данные
                    getAsyncHistoryCarsOfNum(
                        svagon.spinner("value"),
                        function (result) {
                            table_history_car.list = result;
                            table_history_car.loadDataTable(result);
                            table_history_car.obj.draw();
                        }
                    );
                } else {
                    table_history_car.loadDataTable(this.list);
                    table_history_car.obj.draw();
                };
            },
            // Загрузить данные
            loadDataTable: function (data) {
                this.list = data;
                this.obj.clear();
                for (i = 0; i < data.length; i++) {
                    this.obj.row.add({
                        "id": data[i].id,
                        "id_sostav": data[i].id_sostav,
                        "id_arrival": data[i].id_arrival,
                        "num": data[i].num,
                        "dt_uz": data[i].dt_uz,
                        "dt_inp_amkr": data[i].dt_inp_amkr,
                        "dt_out_amkr": data[i].dt_out_amkr,
                        "natur_kis": data[i].natur_kis,
                        "natur_kis_out": data[i].natur_kis_out,
                        "natur": data[i].natur,
                        "dt_create": data[i].dt_create,
                        "user_create": data[i].user_create,
                        "dt_close": data[i].dt_close,
                        "user_close": data[i].user_close,
                        "parent_id": data[i].parent_id,
                        "position": data[i].position,
                    });
                }
                LockScreenOff();
            },
            // Инициализация события выбора поля
            initEventSelect: function () {
                this.html_table.find('tbody')
                    .on('click', 'tr', function () {
                        var id = $(this).attr('id');
                        table_history_car.clearSelect();
                        $(this).addClass('selected');
                        table_history_car.viewTableCarsInpDelivery(id);
                        table_history_car.viewTableCarsOutDelivery(id);
                    });
            },
            // Сбросить выбор поля
            clearSelect: function () {
                this.html_table.find('tbody tr').removeClass('selected');
            },
            initEventSelectChild: function () {
                this.html_table.find('tbody')
                    .on('click', 'td.details-control', function () {
                        var tr = $(this).closest('tr');
                        var row = table_arrival.obj.row(tr);
                        if (row.child.isShown()) {
                            // This row is already open - close it
                            row.child.hide();
                            tr.removeClass('shown');
                        }
                        else {
                            //row.child($('<tr id="detali-transfer"><td colspan="10"><div id="detali' + row.data().id + '" class="detali-operation"></div></td></tr>')).show();
                            row.child('<div id="detali-transfer' + row.data().id + '" class="detali-operation"> ' +
                                '<div id="tabs-detali' + row.data().id + '"> ' +
                                '<ul> ' +
                                '<li><a href="#tabs-detali' + row.data().id + '-1" id="link-tabs-detali' + row.data().id + '-1"></a></li> ' +
                                '<li><a href="#tabs-detali' + row.data().id + '-2" id="link-tabs-detali' + row.data().id + '-2"></a></li> ' +
                                '</ul> ' +
                                '<div id="tabs-detali' + row.data().id + '-1"> ' +
                                //'<table class="display compact" id="table-detali-full" style="width:100%" cellpadding="0"></table> ' +
                                '</div> ' +
                                '<div id="tabs-detali' + row.data().id + '-2"> ' +
                                //'<table class="display compact" id="table-detali-vagon" style="width:100%" cellpadding="0"></table> ' +
                                '</div> ' +
                                '</div> ' +
                                '</div>').show();
                            // Инициализируем
                            $('#link-tabs-detali' + row.data().id + '-1').text(resurses.getText("link_tabs_transfer_detali_1"));
                            $('#link-tabs-detali' + row.data().id + '-2').text(resurses.getText("link_tabs_transfer_detali_2"));
                            $('#tabs-detali' + row.data().id).tabs({
                                collapsible: true,
                            });
                            table_arrival.viewTableChildAllFields(row.data());
                            table_arrival.viewTableChildStatus(row.data());
                            tr.addClass('shown');
                        }
                    });
            },
            // Создать таблицу TableCarsInpDelivery
            viewTableCarsInpDelivery: function (id) {
                OnBegin();
                var target = $("#history-car-sap-inp");
                target.empty();
                target.append(resurses.getText("table_not_data"));
                // Обновим данные
                getAsyncCarsInpDeliveryOfCar(
                    id,
                    function (result) {
                        target.empty();
                        target.append(table_history_car.createTableCarsInpDelivery(result));
                        LockScreenOff();
                    }
                );

            },
            // Сформировать таблицу все поля
            createTableCarsInpDelivery: function (data) {
                if (data == null || data.length == 0) {
                    return resurses.getText("table_not_data")
                }
                var country = rw_reference_country.getCountry(data.id_country);
                var cargo = rw_reference_cargo.getCargo(data.id_cargo);
                var consignee = rw_reference_consignee.getConsignee(data.id_consignee);

                var list_tr = '<tbody>' +
                    '<tr><th>' + resurses.getText("table_field_datetime") + '</th>' + '<td>' + data.datetime + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_composition_index") + '</th>' + '<td>' + data.composition_index + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_id_arrival") + '</th>' + '<td>' + data.id_arrival + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_position") + '</th>' + '<td>' + data.position + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_num_nakl_sap") + '</th>' + '<td>' + data.num_nakl_sap + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_country") + '</th>' + '<td>' + '(' + data.country_code + ')' + (country != null ? (lang == 'en' ? country.country_en : country.country_ru) : '?') + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_weight_cargo") + '</th>' + '<td>' + data.weight_cargo + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_num_doc_reweighing_sap") + '</th>' + '<td>' + data.num_doc_reweighing_sap + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_dt_doc_reweighing_sap") + '</th>' + '<td>' + data.dt_doc_reweighing_sap + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_weight_reweighing_sap") + '</th>' + '<td>' + data.weight_reweighing_sap + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_dt_reweighing_sap") + '</th>' + '<td>' + data.dt_reweighing_sap + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_post_reweighing_sap") + '</th>' + '<td>' + data.post_reweighing_sap + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_cargo") + '</th>' + '<td>' + '(' + data.cargo_code + ')' + (cargo != null ? (lang == 'en' ? cargo.name_full_en : cargo.name_full_ru) : '?') + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_sap_material") + '</th>' + '<td>' + '(' + data.material_code_sap + ')' + data.material_name_sap + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_station_shipment") + '</th>' + '<td>' + data.station_shipment + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_sap_shipment") + '</th>' + '<td>' + '(' + data.station_shipment_code_sap + ')' + data.station_shipment_name_sap + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_consignee") + '</th>' + '<td>' + '(' + data.consignee + ')' + (consignee != null ? (lang == 'en' ? consignee.name_full_en : consignee.name_full_ru) : '?') + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_shop") + '</th>' + '<td>' + '(' + data.shop_code_sap + ')' + data.shop_name_sap + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_new_shop") + '</th>' + '<td>' + '(' + data.new_shop_code_sap + ')' + data.new_shop_name_sap + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_permission_unload_sap") + '</th>' + '<td>' + data.permission_unload_sap + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_step1_sap") + '</th>' + '<td>' + data.step1_sap + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_step2_sap") + '</th>' + '<td>' + data.step2_sap + '</td></tr>' +
                    '</tbody>';
                return '<table class="table-striped table-bordered" id="table-list-history-car-sap-inp" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' + list_tr + '</table>';
            },
            // Создать таблицу TableCarsOutDelivery
            viewTableCarsOutDelivery: function (id) {
                OnBegin();
                var target = $("#history-car-sap-out");
                target.empty();
                target.append(resurses.getText("table_not_data"));
                // Обновим данные
                getAsyncCarsOutDeliveryOfCar(
                    id,
                    function (result) {
                        target.empty();
                        target.append(table_history_car.createTableCarsOutDelivery(result));
                        LockScreenOff();
                    }
                );

            },
            // Сформировать таблицу все поля
            createTableCarsOutDelivery: function (data) {
                if (data == null || data.length == 0) {
                    return resurses.getText("table_not_data")
                }
                var country = rw_reference_country.getCountry(data.id_country_out);
                var cargo = rw_reference_cargo.getCargo(data.id_cargo);
                var station = rw_reference_station.getStation(data.id_station_out);
                var deadlock = rw_deadlock.getDeadlock(data.id_tupik);
                var list_tr = '<tbody>' +
                    '<tr><th>' + resurses.getText("table_field_num_nakl_sap") + '</th>' + '<td>' + data.num_nakl_sap + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_deadlock") + '</th>' + '<td>' + (deadlock!= null ? deadlock.name : '') + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_country_out") + '</th>' + '<td>' + (country != null ? (lang == 'en' ? country.country_en : country.country_ru) : '?') + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_station_out") + '</th>' + '<td>' + (station != null ? station.name : '') + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_note") + '</th>' + '<td>' + data.note + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_cargo") + '</th>' + '<td>' + '(' + data.cargo_code + ')' + (cargo != null ? (lang == 'en' ? cargo.name_full_en : cargo.name_full_ru) : '?') + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_weight_cargo") + '</th>' + '<td>' + data.weight_cargo + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_num_doc_reweighing_sap") + '</th>' + '<td>' + data.num_doc_reweighing_sap + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_dt_doc_reweighing_sap") + '</th>' + '<td>' + data.dt_doc_reweighing_sap + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_weight_reweighing_sap") + '</th>' + '<td>' + data.weight_reweighing_sap + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_dt_reweighing_sap") + '</th>' + '<td>' + data.dt_reweighing_sap + '</td></tr>' +
                    '<tr><th>' + resurses.getText("table_field_post_reweighing_sap") + '</th>' + '<td>' + data.post_reweighing_sap + '</td></tr>' +
                    '</tbody>';
                return '<table class="table-striped table-bordered" id="table-list-history-car-sap-out" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' + list_tr + '</table>';
            },


            createTableStatus: function (data, id) {
                if (data == null || data.length == 0) {
                    return resurses.getText("table_not_data")
                }
                var list_tr = '<thead><tr>' +
                    '<th>' + resurses.getText("table_field_num_car") + '</th>' +
                    '<th>' + resurses.getText("table_field_reult_set_car") + '</th>' +
                    '<th>' + resurses.getText("table_field_reult_upd_car") + '</th>' +
                    '</tr></thead>';
                list_tr += '<tbody>';
                for (i = 0; i < data.length; i++) {
                    list_tr += '<tr>' +
                        //'<td>' + data[i].car + '</td>' +
                        '<td><a id=' + data[i].car + ' name="natur" href="#">' + data[i].car + '</a></td>' +
                        '<td class="' + data[i].car_set + '">' + data[i].car_set + '</td>' +
                        '<td class="' + data[i].car_upd + '">' + data[i].car_upderr + '</td>' +
                        '</tr>';
                }
                list_tr += '</tbody>';
                return '<table class="table-transfer-detali" id="table-detali-status-' + id + '" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' + list_tr + '</table>';
            },
            // Показать все поля
            viewTableChildAllFields: function (data) {
                var target = $("#tabs-detali" + data.id + "-1");
                target.empty();
                var tab = this.createTableAllFields(data);
                target.append(tab);
            },

            // Добавить значения количества вагонов по таблице Prom.Vagon
            viewFieldClose: function (data) {
                if (data.close != null) {
                    var row = table_arrival.obj.row('#' + data.id).index();
                    table_arrival.obj.cell(row, 9).data(data.close_user + " (" + data.close + ")" + (data.close_comment != null ? " - <span>" + data.close_comment + "</span>" : "")).draw();
                }
            },

        },
        // Панель подтверждения удаления
        confirm_delete_panel = {
            html_div: $("#delete-comment"),
            obj: null,
            id_delete: null,
            initObject: function () {
                this.obj = this.html_div.dialog({
                    resizable: false,
                    modal: true,
                    autoOpen: false,
                    height: "auto",
                    width: 300,
                    buttons: {
                        "Удалить": function () {
                            $(this).dialog("close");
                            confirm_delete_panel.Delete(confirm_delete_panel.id_delete);
                        },
                        Cancel: function () {
                            $(this).dialog("close");
                        }
                    }
                });
            },
            Open: function (id) {
                confirm_delete_panel.id_delete = id;
                $('#delete-comment').text(resurses.getText("label_delete_comment")+id);
                this.obj.dialog("option", "title", resurses.getText("confirm_delete_panel_form_text"));
                this.obj.dialog("open");
            },
            Delete: function (id) {
                deleteAsyncSaveCar(
                    id,
                    function (result) {
                        if (Number(result) < 0) {
                            alert(resurses.getText("message_error_delete_vagon") + data);
                        }
                        table_history_car.viewTable(true);
                    });
            }
        }
    //-----------------------------------------------------------------------------------------
    // Функции
    //-----------------------------------------------------------------------------------------
    var outVal = function (i) {
        return i != null ? Number(i) : '';
    };

    //-----------------------------------------------------------------------------------------
    // Инициализация объектов
    //-----------------------------------------------------------------------------------------
    resurses.initObject("/railway/Scripts/RW/rw.json",
        function () {
            // Загружаем дальше
            //$('#label-select-vagon').text(resurses.getText("label_select_vagon"));
            //bt_searsh.text(resurses.getText("button_to_searsh"));
            //$('#searsh').text(resurses.getText("button_to_searsh"));
            //$('#to-excel').text(resurses.getText("button_to_excel"));
            confirm_delete_panel.initObject();      // Панель подтверждения удаления

            rw_reference_country.initObject();      // Справочник стран
            rw_reference_cargo.initObject();        // Справочник грузов
            rw_reference_consignee.initObject();    // Справочник грузоотправителей
            rw_reference_station.initObject();      // Справочник станций

            rw_deadlock.initObject();                // Справочник тупики

            table_history_car.initObject();
            accordion_history.initObject();

            tab_type_vagons.initObject(); // Типы закладок отчетов

            //var dd = allVars.length;
            if (allVars.num != null) {
                svagon.spinner("value", allVars.num);
                table_history_car.viewTable(true); // Первый запуск
            } else {
                //!!!! тест
                svagon.spinner("value", 66240128)

            }


        }); // локализация



});
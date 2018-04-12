$(function () {
    //-----------------------------------------------------------------------------------------
    // Объявление глобальных переменных
    //-----------------------------------------------------------------------------------------
    var lang = $.cookie('lang'),
        //------------------------------------
        // Ресурс локализации 
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
        //------------------------------------
        // Элементы 
        //  - панель отображения
        label_view_inp_cargo = $('<label for="view-inp-cargo"></label>'),
        checkbox_view_inp_cargo = $('<input type="checkbox" name="view" id="view-inp-cargo" checked="checked" >'),
        label_view_out_cargo = $('<label for="view-out-cargo"></label>'),
        checkbox_view_out_cargo = $('<input type="checkbox" name="view" id="view-out-cargo" checked="checked" >'),
        label_view_inp_sap = $('<label for="view-inp-sap"></label>'),
        checkbox_view_inp_sap = $('<input type="checkbox" name="view" id="view-inp-sap">'),
        label_view_out_sap = $('<label for="view-out-sap"></label>'),
        checkbox_view_out_sap = $('<input type="checkbox" name="view" id="view-out-sap">'),
        label_view_email = $('<label for="view-email"></label>'),
        checkbox_view_email = $('<input type="checkbox" name="view" id="view-email">'),
        //  - панель информации
        label_info = $('<label id="view-info"></label>'),
        //  - режимы работы 
        label_mode_view = $('<label for="mode-view"></label>'),
        radio_mode_view = ('<input type="radio" name="mode" id="mode-view" checked="checked" >'),
        label_mode_manevr = $('<label for="mode-manevr"></label>'),
        radio_mode_manevr = $('<input type="radio" name="mode" id="mode-manevr">'),
        label_mode_sending = $('<label for="mode-sending"></label>'),
        radio_mode_sending = $('<input type="radio" name="mode" id="mode-sending">'),
        label_mode_correction = $('<label for="mode-correction"></label>'),
        radio_mode_correction = $('<input type="radio" name="mode" id="mode-correction">'),
        label_mode_statepark = $('<label for="mode-statepark"></label>'),
        radio_mode_statepark = $('<input type="radio" name="mode" id="mode-statepark">'),
        label_mode_accept = $('<label for="mode-accept"></label>'),
        radio_mode_accept = $('<input type="radio" name="mode" id="mode-accept">'),
        label_mode_transit = $('<label for="mode-transit"></label>'),
        radio_mode_transit = $('<input type="radio" name="mode" id="mode-transit">'),

        panel_detali = $('#detali-group'), // Панель отображения данных по группам
        //------------------------------------
        // Панель режимов 
        panel_mode = {
            active: 0,
            html_div: $('<div class="dt-buttons setup-operation" id="property_view_mode"></div>'),
            initPanel: function (view, manevr, sending, accept, transit, correction, statepark) {
                this.html_div.empty();
                if (view) {
                    this.html_div.append(label_mode_view.text(resurses.getText("label_mode_view"))).append(radio_mode_view);
                };
                if (manevr) {
                    this.html_div.append(label_mode_manevr.text(resurses.getText("label_mode_manevr"))).append(radio_mode_manevr);
                };
                if (sending) {
                    this.html_div.append(label_mode_sending.text(resurses.getText("label_mode_sending"))).append(radio_mode_sending);
                };
                if (accept) {
                    this.html_div.append(label_mode_accept.text(resurses.getText("label_mode_accept"))).append(radio_mode_accept);
                };
                if (transit) {
                    this.html_div.append(label_mode_transit.text(resurses.getText("label_mode_transit"))).append(radio_mode_transit);
                };
                if (correction) {
                    this.html_div.append(label_mode_correction.text(resurses.getText("label_mode_correction"))).append(radio_mode_correction);
                };
                if (statepark) {
                    this.html_div.append(label_mode_statepark.text(resurses.getText("label_mode_statepark"))).append(radio_mode_statepark);
                };
                this.html_div.controlgroup();
                // Выберем режим просмотра
                $("#mode-view").prop('checked', true);
                //panel_mode.activeView();
                // определим событие выбора режима
                $("[name='mode']").on("change", panel_mode.selectToggle);
                panel_mode.html_div.controlgroup("refresh"); // Обновим 
            },
            selectToggle: function (e) {
                var target = $(e.target);
                if (target.is("#mode-view")) {
                    panel_mode.activeView();
                }
                if (target.is("#mode-manevr")) {
                    panel_mode.activeManevr();
                }
                if (target.is("#mode-sending")) {
                    panel_mode.activeSending();
                }
                if (target.is("#mode-correction")) {
                    panel_mode.activeCorrection();
                }
                if (target.is("#mode-statepark")) {
                    panel_mode.activeStatepark();
                }
                if (target.is("#mode-accept")) {
                    panel_mode.activeAccept();
                }
                if (target.is("#mode-transit")) {
                    panel_mode.activeTransit();
                }
            },
            show: function () {
                this.html_div.show();
            },
            hide: function () {
                this.html_div.hide();
            },
            activeView: function () {
                panel_mode.active = 0;
                //panel.manevr.obj.hide();
                //panel.sent.obj.hide();
                //panel.arrival.obj.hide();
                //panel.accept.obj.hide();
                //cars.clearSelect();
                //panel_detali.removeClass();
            },
            activeManevr: function () {
                panel_mode.active = 1;
                //panel.manevr.obj.show(); panel.manevr.initPanel();
                //panel.sent.obj.hide();
                //panel.arrival.obj.hide();
                //panel.accept.obj.hide();
                ////panel.transit.obj.hide();
                //cars.clearSelect();
                //panel_detali.removeClass();
                //panel_detali.addClass('mode-manevr');
            },
            activeSending: function () {
                panel_mode.active = 2;
                //panel.manevr.obj.hide();
                //panel.sent.obj.show(); panel.sent.initPanel();
                //panel.arrival.obj.hide();
                //panel.accept.obj.hide();
                ////panel.transit.obj.hide();
                //cars.clearSelect();
                //panel_detali.removeClass();
                //panel_detali.addClass('mode-sending');
            },
            activeAccept: function () {
                panel_mode.active = 3;
                //panel.manevr.obj.hide();
                //panel.sent.obj.hide();
                //panel.arrival.obj.hide();
                //panel.accept.obj.show(); panel.accept.initPanel();
                ////panel.transit.obj.hide();
                //cars.clearSelect();
                //panel_detali.removeClass();
                //panel_detali.addClass('mode-accept');
            },
            activeTransit: function () {
                panel_mode.active = 4;
                //panel.manevr.obj.hide();
                //panel.sent.obj.hide();
                //panel.arrival.obj.hide();
                //panel.accept.obj.show(); panel.accept.initPanel();
                ////panel.transit.obj.show(); panel.transit.initPanel();
                //cars.clearSelect();
                //panel_detali.removeClass();
                //panel_detali.addClass('mode-transit');
            },
            activeCorrection: function () {
                panel_mode.active = 5;
                //panel.manevr.obj.hide();
                //panel.sent.obj.hide();
                //panel.arrival.obj.hide();
                //panel.accept.obj.hide();
                ////panel.transit.obj.hide();
                //cars.clearSelect();
                //panel_detali.removeClass();
                //panel_detali.addClass('mode-correction');
            },
            activeStatepark: function () {
                panel_mode.active = 6;
                //panel.manevr.obj.hide();
                //panel.sent.obj.hide();
                //panel.arrival.obj.hide();
                //panel.accept.obj.hide();
                ////panel.transit.obj.hide();
                //cars.clearSelect();
                //panel_detali.removeClass();
                //panel_detali.addClass('mode-statepark');
            },
        },
        //------------------------------------
        // Панель нстроек отображения информации
        panel_view = {
            html_div: $('<div class="dt-buttons setup-operation" id="property_view_cars"></div>'),
            handleToggle: function (e) {
                var target = $(e.target);
                if (target.is("#view-inp-cargo")) {
                    table_cars_details.enableColumsInpCargo(target[0].checked);
                }
                if (target.is("#view-inp-sap")) {
                    table_cars_details.enableColumsInpSAP(target[0].checked);
                }
                if (target.is("#view-out-cargo")) {
                    table_cars_details.enableColumsOutCargo(target[0].checked);
                }
                if (target.is("#view-out-sap")) {
                    table_cars_details.enableColumsOutSAP(target[0].checked);
                }
                if (target.is("#view-email")) {
                    table_cars_details.enableColumsEmail(target[0].checked);
                }
            },
            initPanel: function () {
                this.html_div.empty();
                this.html_div
                    .append(label_view_inp_cargo.text(resurses.getText("label_view_inp_cargo"))).append(checkbox_view_inp_cargo)
                    .append(label_view_inp_sap.text(resurses.getText("label_view_inp_sap"))).append(checkbox_view_inp_sap)
                    .append(label_view_out_cargo.text(resurses.getText("label_view_out_cargo"))).append(checkbox_view_out_cargo)
                    .append(label_view_out_sap.text(resurses.getText("label_view_out_sap"))).append(checkbox_view_out_sap)
                    .append(label_view_email.text(resurses.getText("label_view_email"))).append(checkbox_view_email);
                this.html_div.controlgroup();
                checkbox_view_inp_cargo.on("change", panel_view.handleToggle);
                checkbox_view_inp_sap.on("change", panel_view.handleToggle);
                checkbox_view_out_cargo.on("change", panel_view.handleToggle);
                checkbox_view_out_sap.on("change", panel_view.handleToggle);
                checkbox_view_email.on("change", panel_view.handleToggle);

                //checkbox_view_inp_sap.checked = false;

            },
            show: function () {
                this.html_div.show();
            },
            hide: function () {
                this.html_div.hide();
            },
        },
        //------------------------------------
        // Панель отображения информации
        panel_info = {
            html_div: $('<div class="dt-buttons setup-operation" id="property_info"></div>'),
            initPanel: function () {
                this.html_div.append(label_info.text('Информация'));
            },
            viewInfoText: function (value) {
                label_info.text(value);
            },
        },
        //panel_manevr = {
        //    html_div: $('<div class="dt-buttons setup-operation" id="property_manevr_operation"></div>'),
        //    initPanel: function () {
        //        this.obj.empty();
        //        panel.element.button_manevr_select_all.on('click', function () {
        //            cars.allSelect();
        //        });
        //        panel.element.button_manevr_clear_all.on('click', function () {
        //            cars.clearSelect();
        //        });
        //        this.obj.append(panel.element.button_manevr_select_all)
        //                .append(panel.element.button_manevr_clear_all)
        //                .append(panel.element.label_gorlov_manevr)
        //                .append(panel.element.select_manevr_side)
        //                .append(panel.element.label_way_manevr)
        //                .append(panel.element.select_manevr_way)
        //                .append(panel.element.button_ok_manevr);
        //        initSelect(panel.element.select_manevr_side, { width: 150 }, side.list, null, 0, function (event, ui) { event.preventDefault(); }, null);
        //        initSelect(panel.element.select_manevr_way, { width: 300 }, ways.list, function (row) { return { value: row.id_way, text: row.num + '-' + row.name }; }, -1, function (event, ui) { event.preventDefault(); }, null);

        //    },
        //}
        //------------------------------------
        // Панель управления и отображения
        panel = {
            initObject: function (obj) {
                // Всегда показывать
                panel_view.initPanel();
                panel_info.initPanel();
                //obj.prepend(panel.accept.obj);
                //obj.prepend(panel.sent.obj);
                //obj.prepend(panel.manevr.obj);
                obj.prepend(panel_info.html_div);
                obj.prepend(panel_view.html_div);
                obj.prepend(panel_mode.html_div);
                //obj.prepend(panel.transit.obj);
            },
            activePanel: function (active_group) {
                panel_view.show();
                panel_mode.hide();
                //....
                switch (active_group) {
                    case 0:
                        panel_mode.show();
                        panel_mode.initPanel(true, true, true, false, false, true, true);
                        break;
                    case 1:
                        panel_mode.show();
                        panel_mode.initPanel(true, false, false, true, true, true, true);
                        break;
                    case 2:
                        panel_mode.show();
                        panel_mode.initPanel(true, false, false, true, true, true, true);
                        break;
                    case 3:
                        panel_mode.show();
                        panel_mode.initPanel(true, false, false, true, true, false, false);
                        break;
                    case 4:
                        panel_mode.show();
                        panel_mode.initPanel(true, false, false, true, false, false, false);
                        break;
                    case 5:
                        panel_mode.hide();
                        break;
                    default:
                        // Группы закрыты

                        break;
                }
            }
        },
        //------------------------------------
        // Группы спиков
        tab_group_list = {
            html_div: $("#group-list"),
            active: 0,
            initObject: function () {
                this.html_div.accordion({
                    collapsible: true,
                    heightStyle: "content",
                    activate: function (event, ui) {
                        tab_group_list.active = tab_group_list.html_div.accordion("option", "active");
                        tab_group_list.viewGroup(tab_group_list.active, station, false);
                    },
                });
                group_list_ways.hide();
                group_list_wo.hide();
                group_list_shops.hide();
                group_list_arrival.hide();
                group_list_arrival_uz.hide();
                group_list_sending.hide();
                tab_group_list.viewGroup(tab_group_list.active, station, false);
            },
            //Активация групп списков, в соответсвии с указаной станции
            activeOfStation: function (station) {
                // Показать пути
                group_list_ways.show(station.id, false);
                if (station != null) {
                    if (station.exit_uz) {
                        group_list_arrival_uz.show(station, false);
                    } else { group_list_arrival_uz.hide() }
                }
            },
            //Активировать группу
            viewGroup: function (active, station, data_refresh) {
                panel.activePanel(active);
                switch (active) {
                    case 0:
                        //viewGroupWays(station_id, data_refresh);
                        group_list_ways.show(station.id, data_refresh);
                        break;
                    case 1:
                        //viewGroupWagonOverturns(station_id, data_refresh);
                        break;
                    case 2:
                        //viewGroupShops(station_id, data_refresh);
                        break;
                    case 3:
                        //viewGroupArrivalAMKR(station_id, data_refresh);
                        break;
                    case 4:
                        group_list_arrival_uz.show(station, data_refresh);
                        //viewGroupArrivalUZ(station_id, data_refresh);
                        break;
                    case 5:
                        //viewGroupSending(station_id, data_refresh);
                        break;

                    default:
                        // Группы закрыты
                        table_cars_details.enableTable(-1);
                        //panel_detali.removeClass(); // Убрать подкраску режимов
                        break;
                }
            },
        },
    //------------------------------------
    // class CarsOnWay
    cars_on_way = {
        id: null,
        num: null,
        name_ru: null,
        name_en: null,
        cars: null,
        capacity: null,
    },
    //------------------------------------
    // Таблица вагоны на пути станции
    table_ways = {
        name: 'ways',
        station_id: null,
        select: cars_on_way,
        html_table: $('#table-list-ways'),
        html_div_table: null,
        obj: null,
        list: null,
        initTable: function () {
            this.obj = this.html_table.DataTable({
                "paging": false,
                "ordering": false,
                "info": false,
                "select": false,
                "filter": false,
                language: {
                    emptyTable: resurses.getText("table_message_emptyTable"),
                },
                jQueryUI: true,
                "createdRow": function (row, data, index) {
                    $(row).attr('id', data.id);
                    if (table_ways.select != null && data.id == table_ways.select.id) {
                        $(row).addClass('selected');
                    }
                },
                columns: [
                    { data: "num", title: resurses.getText("table_field_num") },
                    { data: "name", title: resurses.getText("table_field_name") },
                    { data: "cars", title: resurses.getText("table_field_cars_count"), width: "30px" },
                    { data: "capacity", title: resurses.getText("table_field_cars_capacity"), width: "30px" },
                ],
            });
            this.html_div_table = $('DIV#table-list-ways_wrapper');
            this.initEventSelect();
            this.html_div_table.hide();
        },
        initEventSelect: function () {
            this.html_table.find('tbody')
                    .on('click', 'tr', function () {
                        table_ways.clearSelect();
                        $(this).addClass('selected');
                        var id = Number($(this).attr("id"));
                        if (id >= 0) {
                            table_ways.setSelectWay(id);
                        }
                        table_cars_details.viewTable(table_ways, false);
                    });
        },
        setSelectWay: function (id) {
            var way = getObjects(table_ways.list, 'id', id)
            if (way != null && way.length > 0) {
                table_ways.select = way[0];
            }
        },
        clearSelect: function () {
            this.html_table.find('tbody tr').removeClass('selected');
        },
        loadData: function (data) {
            this.list = data;
            this.obj.clear();
            for (i = 0; i < data.length; i++) {
                // Добавим данные о станциях
                this.obj.row.add({
                    "id": data[i].id,
                    "num": data[i].num,
                    "name": lang == 'en' ? data[i].name_ru : data[i].name_en,
                    "cars": data[i].cars,
                    "capacity": data[i].capacity,
                });
            };
            this.obj.draw();
        },
        enableTable: function (length) {
            if (length > 0) {
                this.html_div_table.show();
            } else {
                this.html_div_table.hide();
            }
        },
        viewTable: function (station_id, data_refresh, callback) {

            if (this.list == null | this.station_id != station_id | data_refresh == true) {
                // Обновим данные
                this.station_id = station_id;
                getAsyncCountCarsOnWayOfStation(
                    station_id,
                    function (result) {
                        table_ways.loadData(result);
                        table_ways.enableTable(result.length);
                        table_cars_details.viewTable(table_ways, false);
                        if (typeof callback === 'function') {
                            callback(result.length);
                        }
                    }
                    );
            } else {
                this.enableTable(this.list.length);
                table_cars_details.viewTable(table_ways, false);
                if (typeof callback === 'function') {
                    callback(this.list.length);
                }
            };
        },
        clearListSelect: function () {
            this.list = null;
            this.select = null;
        }
    },
    //------------------------------------
    // группа список путей
    group_list_ways = {
        html_div: $("#group-list-ways"),
        hide: function () {
            this.html_div.hide();
        },
        show: function (id_station, refresh) {
            this.html_div.show();
            table_ways.viewTable(id_station, false, null);
        },
    },
    //------------------------------------
    // группа список вогоноопрокидов
    group_list_wo = {
        html_div: $("#group-list-wagonoverturns"),
        hide: function () {
            this.html_div.hide();
        },
        show: function () {
            this.html_div.show();
        },
    },
    //------------------------------------
    // группа список цехов
    group_list_shops = {
        html_div: $("#group-list-shops"),
        hide: function () {
            this.html_div.hide();
        },
        show: function () {
            this.html_div.show();
        },
    },
    //------------------------------------
    // группа список прибытия по АМКР
    group_list_arrival = {
        html_div: $("#group-list-arrival-amkr"),
        hide: function () {
            this.html_div.hide();
        },
        show: function () {
            this.html_div.show();
        },
    },
    //------------------------------------
    // class ArrivalSostav
    arrival_sostav = {
        id_sostav: null,
        id_arrival: null,
        index: null,
        dt_inp_station: null,
        id_station: null,
        name_ru: null,
        name_en: null,
        id_way: null,
        cars: null,
    },
    //------------------------------------
    // таблица прибытия на станци УЗ
    table_arrival_uz = {
        name: 'arrival_uz',
        station_id: null,
        select: arrival_sostav,
        html_table: $('#table-list-arrival-uz'),
        html_div_table: null,
        obj: null,
        list: null,
        initTable: function () {
            this.obj = this.html_table.DataTable({
                "paging": false,
                "ordering": false,
                "info": false,
                "select": false,
                "filter": false,
                language: {
                    emptyTable: resurses.getText("table_message_emptyTable"),
                },
                jQueryUI: true,
                "createdRow": function (row, data, index) {
                    $(row).attr('id', data.id_sostav);
                    if (table_arrival_uz.select != null && data.id_sostav == table_arrival_uz.select.id_sostav) {
                        $(row).addClass('selected');
                    }
                },
                columns: [
                    { data: "station", title: "Станция" },
                    { data: "index", title: "Состав" },
                    { data: "dt_inp_station", title: "Дата и время" },
                    { data: "cars", title: "Кол. ваг.", width: "30px" },
                ],
            });
            this.html_div_table = $('DIV#table-list-arrival-uz_wrapper');
            this.initEventSelect();
            this.html_div_table.hide();
        },
        initEventSelect: function () {
            this.html_table.find('tbody')
                    .on('click', 'tr', function () {
                        table_arrival_uz.clearSelect();
                        $(this).addClass('selected');
                        var id = Number($(this).attr("id"));
                        if (id >= 0) {
                            table_arrival_uz.setSelectSostav(id);
                        }
                        table_cars_details.viewTable(table_arrival_uz, false);
                    });
        },
        setSelectSostav: function (id) {
            var sostav = getObjects(table_arrival_uz.list, 'id_sostav', id)
            if (sostav != null && sostav.length > 0) {
                table_arrival_uz.select = sostav[0];
            }
        },
        clearSelect: function () {
            this.html_table.find('tbody tr').removeClass('selected');
        },
        loadData: function (data) {
            this.list = data;
            this.obj.clear();
            for (i = 0; i < data.length; i++) {
                // Добавим данные о станциях
                this.obj.row.add({
                    "station": lang == 'en' ? data[i].name_en : data[i].name_ru,
                    "id_sostav": data[i].id_sostav,
                    "index": data[i].index,
                    "dt_inp_station": data[i].dt_inp_station,
                    "cars": data[i].cars,
                });
            };
            this.obj.draw();
        },
        enableTable: function (length) {
            if (length > 0) {
                this.html_div_table.show();
            } else {
                this.html_div_table.hide();
            }
        },
        // Показать таблицу 
        viewTable: function (station, data_refresh, callback) {
            if (data_refresh == true || table_arrival_uz.station_id == null || table_arrival_uz.station_id != station.id || table_arrival_uz.list == null) {
                // Обновим данные
                table_arrival_uz.station_id = station.id;
                var list_id_uz = '';
                for (i = 0; i < station.arrival_uz.length; i++) {
                    list_id_uz += station.arrival_uz[i].id;

                    if (i < station.arrival_uz.length - 1) {
                        list_id_uz += ',';
                    }
                }
                getAsyncArrivalSostavOfStationsUZ(
                    list_id_uz,
                    function (result) {
                        table_arrival_uz.loadData(result);
                        table_arrival_uz.enableTable(result.length);
                        table_cars_details.viewTable(table_arrival_uz, false);
                        if (typeof callback === 'function') {
                            callback(result.length);
                        }
                    }
                    );
            } else {
                this.enableTable(this.list.length);
                table_cars_details.viewTable(table_arrival_uz, false);
                if (typeof callback === 'function') {
                    callback(this.list.length);
                }
            };
        },
        clearListSelect: function () {
            this.list = null;
            this.select = null;
        }
    },
    //------------------------------------
    // группа список прибытия по УЗ
    group_list_arrival_uz = {
        html_div: $("#group-list-arrival-uz"),
        hide: function () {
            this.html_div.hide();
        },
        show: function (station, refresh) {
            this.html_div.show();
            table_arrival_uz.viewTable(station, false, null);
        },

    },
    //------------------------------------
    // группа список прибытия по УЗ
    group_list_sending = {
        html_div: $("#group-list-sending"),
        hide: function () {
            this.html_div.hide();
        },
        show: function () {
            this.html_div.show();
        },
    },
    //------------------------------------
    // Таблица вагоны детально
    table_cars_details = {
        select: null,
        html_table: $('#table-list-cars'),
        html_div_table: null,
        obj: null,
        list: null,
        group: null,                            // Активная группа списка
        group_select: null,                     // id выбранного элемента группы
        initTable: function () {
            this.obj = this.html_table.DataTable({
                "paging": false,
                "ordering": true,
                "info": false,
                "select": false,
                "filter": true,
                "scrollY": "600px",
                "scrollX": true,
                language: {
                    emptyTable: resurses.getText("table_message_emptyTable"),
                },
                jQueryUI: true,
                "createdRow": function (row, data, index) {
                    $(row).attr('id', data.id_operations);
                    if (data.id_operations == this.select) {

                    }
                },
                columns: [
                    { data: "position", title: resurses.getText("table_field_num"), orderable: false, searchable: false },
                    { data: "num", title: resurses.getText("table_field_car_num"), orderable: false, searchable: true },
                    // - Тип вагона, владелец, страна
                    { data: "type_cars_abr", title: resurses.getText("table_field_car_type"), orderable: false, searchable: false },
                    { data: "owner_name", title: resurses.getText("table_field_car_owner"), width: "150px", orderable: false, searchable: false },
                    { data: "country", title: resurses.getText("table_field_car_country"), orderable: false, searchable: false },
                    { data: "status", title: resurses.getText("table_field_car_status"), orderable: false, searchable: false },
                    // - Входящие поставки
                    { data: "dt_uz", title: resurses.getText("table_field_car_dt_uz"), width: "150px", orderable: false, searchable: false },
                    { data: "dt_inp_amkr", title: resurses.getText("table_field_car_dt_inp_amkr"), width: "150px", orderable: false, searchable: false },
                    { data: "conditions", title: resurses.getText("table_field_car_conditions"), orderable: false, searchable: false },
                    { data: "inp_type_cargo", title: resurses.getText("table_field_car_inp_type_cargo"), orderable: false, searchable: false },
                    { data: "inp_cargo", title: resurses.getText("table_field_car_inp_cargo"), width: "300px", orderable: false, searchable: false },
                    { data: "inp_weight_cargo", title: resurses.getText("table_field_car_inp_weight_cargo"), orderable: false, searchable: false },
                    { data: "inp_station_shipment", title: resurses.getText("table_field_car_inp_station_shipment"), width: "200px", orderable: false, searchable: false },
                    { data: "inp_consignee_name_abr", title: resurses.getText("table_field_car_inp_consignee"), orderable: false, searchable: false },
                    { data: "inp_sostav_index", title: resurses.getText("table_field_car_inp_sostav_index"), orderable: false, searchable: false },
                    // - Входящие САП
                    { data: "inp_num_nakl_sap", title: resurses.getText("table_field_car_inp_num_nakl_sap"), orderable: false, searchable: false },
                    // - САП перевеска
                    { data: "inp_num_doc_reweighing_sap", title: resurses.getText("table_field_car_inp_num_doc_reweighing_sap"), orderable: false, searchable: false },
                    { data: "inp_dt_doc_reweighing_sap", title: resurses.getText("table_field_car_inp_dt_doc_reweighing_sap"), orderable: false, searchable: false },
                    { data: "inp_weight_reweighing_sap", title: resurses.getText("table_field_car_inp_weight_reweighing_sap"), orderable: false, searchable: false },
                    { data: "inp_dt_reweighing_sap", title: resurses.getText("table_field_car_inp_dt_reweighing_sap"), orderable: false, searchable: false },
                    { data: "inp_post_reweighing_sap", title: resurses.getText("table_field_car_inp_post_reweighing_sap"), orderable: false, searchable: false },
                    // - САП материалы
                    { data: "inp_material_code_sap", title: resurses.getText("table_field_car_inp_material_code_sap"), orderable: false, searchable: false },
                    { data: "inp_material_name_sap", title: resurses.getText("table_field_car_inp_material_name_sap"), orderable: false, searchable: false },
                    // - САП станция отправитель
                    { data: "inp_station_shipment_code_sap", title: resurses.getText("table_field_car_inp_station_shipment_code_sap"), orderable: false, searchable: false },
                    { data: "inp_station_shipment_name_sap", title: resurses.getText("table_field_car_inp_station_shipment_name_sap"), orderable: false, searchable: false },
                    // - САП цех получатель
                    { data: "inp_shop_code_sap", title: resurses.getText("table_field_car_inp_shop_code_sap"), orderable: false, searchable: false },
                    { data: "inp_shop_name_sap", title: resurses.getText("table_field_car_inp_shop_name_sap"), orderable: false, searchable: false },
                    { data: "inp_new_shop_code_sap", title: resurses.getText("table_field_car_inp_new_shop_code_sap"), orderable: false, searchable: false },
                    { data: "inp_new_shop_name_sap", title: resurses.getText("table_field_car_inp_new_shop_name_sap"), orderable: false, searchable: false },

                    // - письма
                    //-..................
                    // - Исходящие поставки
                    //-..................

                ],
            });
            this.html_div_table = $('DIV#table-list-cars_wrapper');
            panel.initObject(this.html_div_table); // Инициализируем панель 
            this.initEventSelect();
            // Отображение полей по умолчанию
            var view_inp_cargo = checkbox_view_inp_cargo.attr('checked');
            this.enableColumsInpCargo(view_inp_cargo != null ? view_inp_cargo : false);
            var view_out_cargo = checkbox_view_out_cargo.attr('checked');
            this.enableColumsOutCargo(view_out_cargo != null ? view_out_cargo : false);
            var view_inp_sap = checkbox_view_inp_sap.attr('checked');
            this.enableColumsInpSAP(view_inp_sap != null ? view_inp_sap : false);
            var view_out_sap = checkbox_view_out_sap.attr('checked');
            this.enableColumsOutSAP(view_out_sap != null ? view_out_sap : false);
            var view_email = checkbox_view_email.attr('checked');
            this.enableColumsEmail(view_email != null ? view_email : false);

            this.html_div_table.hide();
        },
        initEventSelect: function () {
            //this.html_table.find('tbody')
            //        .on('click', 'tr', function () {
            //            table_ways.clearSelect();
            //            $(this).addClass('selected');
            //            var id = Number($(this).attr("id"));
            //            if (id >= 0) {
            //                table_ways.setSelectWay(id);
            //            }
            //            //table_arrival_uz.getSelectObj(arrival_uz.select);
            //            //cars.viewTable(arrival_uz, false);
            //        });
        },
        setSelectWay: function (id) {
            var way = getObjects(table_ways.list, 'id', id)
            if (way != null && way.length > 0) {
                table_ways.select = way[0];
            }
        },
        clearSelect: function () {
            this.html_table.find('tbody tr').removeClass('selected');
        },
        allSelect: function () {
            this.table.find('tbody tr').addClass('selected');
        },
        sortPosition: function () {
            if (tab_group_list.active == 0) {
                if (rw_side.select_side.id == rw_side.out_default) {
                    this.obj.order([0, 'desc']);
                } else {
                    this.obj.order([0, 'asc']);
                }
            } else {
                this.obj.order([0, 'asc']);
            }
            this.obj.draw();
        },
        loadData: function (data) {
            this.list = data;
            this.obj.clear();
            for (i = 0; i < data.length; i++) {
                // Добавим данные о станциях
                this.obj.row.add({
                    "id_operations": data[i].id_operations, //0
                    "position": data[i].position,
                    "num": data[i].num,
                    "type_cars_abr": lang == 'en' ? data[i].type_cars_abr_en : data[i].type_cars_abr_ru,
                    "owner_name": data[i].owner_name,
                    "country": lang == 'en' ? data[i].country_en : data[i].country_ru,
                    "status": lang == 'en' ? data[i].status_en : data[i].status_ru,
                    "conditions": lang == 'en' ? data[i].conditions_en : data[i].conditions_ru,
                    "inp_type_cargo": lang == 'en' ? data[i].inp_type_cargo_en : data[i].inp_type_cargo_ru,
                    "inp_cargo": lang == 'en' ? data[i].inp_cargo_en : data[i].inp_cargo_ru,
                    "inp_consignee_name_abr": lang == 'en' ? data[i].inp_consignee_name_abr_en : data[i].inp_consignee_name_abr_ru,
                    "dt_uz": data[i].dt_uz,
                    "dt_inp_amkr": data[i].dt_inp_amkr,
                    "inp_station_shipment": data[i].inp_station_shipment,
                    "inp_weight_cargo": data[i].inp_weight_cargo,
                    "inp_sostav_index": data[i].inp_sostav_index,
                    "inp_num_nakl_sap": data[i].inp_num_nakl_sap,
                    "inp_num_doc_reweighing_sap": data[i].inp_num_doc_reweighing_sap,
                    "inp_dt_doc_reweighing_sap": data[i].inp_dt_doc_reweighing_sap,
                    "inp_weight_reweighing_sap": data[i].inp_weight_reweighing_sap,
                    "inp_dt_reweighing_sap": data[i].inp_dt_reweighing_sap,
                    "inp_post_reweighing_sap": data[i].inp_post_reweighing_sap,
                    "inp_material_code_sap": data[i].inp_material_code_sap,
                    "inp_material_name_sap": data[i].inp_material_name_sap,
                    "inp_station_shipment_code_sap": data[i].inp_station_shipment_code_sap,
                    "inp_station_shipment_name_sap": data[i].inp_station_shipment_name_sap,
                    "inp_shop_code_sap": data[i].inp_shop_code_sap,
                    "inp_shop_name_sap": data[i].inp_shop_name_sap,
                    "inp_new_shop_code_sap": data[i].inp_new_shop_code_sap,
                    "inp_new_shop_name_sap": data[i].inp_new_shop_name_sap,
                });
            };
            // Перестроить последовательность нумерации вагонов
            this.sortPosition();
            //this.obj.draw(); - это делается в this.sortPosition();
        },
        // Показать таблицу ессли есть значения
        enableTable: function (length) {
            if (length >= 0) {
                this.html_div_table.show();
            } else {
                this.html_div_table.hide();
            }
        },
        viewTable: function (obj_select, data_refresh) {
            if (typeof obj_select == 'object') {
                // Показать выбранный путь
                if (obj_select.name == 'ways' & tab_group_list.active == 0) {
                    if (table_cars_details.list == null | data_refresh == true | table_cars_details.group != 'ways' | (table_cars_details.group == 'ways' & table_cars_details.group_select != obj_select.select)) {
                        table_cars_details.group_select = obj_select.select;
                        table_cars_details.group = obj_select.name;
                        // Загружаем
                        getAsyncCarsOnWay(
                            (obj_select.select != null ? obj_select.select.id : 0),
                            (rw_side.select_side.id == rw_side.out_default ? 1 : 0),
                            function (result) {
                                //panel.info.viewInfo(obj_select);
                                table_cars_details.loadData(result);
                                table_cars_details.enableTable(result.length);
                            });
                    } else {
                        //cars.loadData(cars.list);
                        table_cars_details.enableTable(table_cars_details.list.length);
                    }
                }
                // Показать выбранное прибытие
                if (obj_select.name == 'arrival_uz' & tab_group_list.active == 4) {
                    if (table_cars_details.list == null | data_refresh == true | table_cars_details.group != 'arrival_uz' | (table_cars_details.group == 'arrival_uz' & table_cars_details.group_select != obj_select.select)) {
                        table_cars_details.group_select = obj_select.select;
                        table_cars_details.group = obj_select.name;
                        // Загружаем
                        getAsyncCarsOnWayUZ(
                            (obj_select.select != null ? obj_select.select.id_way : 0),
                            (obj_select.select != null ? obj_select.select.id_arrival : 0),
                            (rw_side.select_side.id == rw_side.out_default ? 1 : 0),
                            function (result) {
                                //panel.info.viewInfo(obj_select);
                                table_cars_details.loadData(result);
                                table_cars_details.enableTable(result.length);
                            });
                    } else {
                        //cars.loadData(cars.list);
                        table_cars_details.enableTable(table_cars_details.list.length);
                    }
                }




            }
        },
        enableColumsInpCargo: function (view) {
            this.obj.columns([9, 10, 11, 13]).visible(view, true);
            this.obj.draw(false);
        },
        enableColumsInpSAP: function (view) {
            this.obj.columns([15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28]).visible(view, true);
            this.obj.draw(false);
        },
        enableColumsOutCargo: function (view) {
            //this.obj.columns([]).visible(view, true);
            //this.obj.draw(false);
        },
        enableColumsOutSAP: function (view) {
            //this.obj.columns([]).visible(view, true);
            //this.obj.draw(false);
        },
        enableColumsEmail: function (view) {
            //this.obj.columns([]).visible(view, true);
            //this.obj.draw(false);
        },
        clearListSelect: function () {
            //this.list = null;
            //this.select = null;
        }
    },
    //------------------------------------
    //class station
    station = {
        id: -1,
        name: null,
        view: null,
        exit_uz: null,
        station_uz: null,
        id_rs: -1,
        id_kis: -1,
        default_side: null,
        code_uz: null,
        arrival_uz: null
    },
    //------------------------------------
    // Объект станции системы
    rw_stations = {
        html_select: $('select[name ="station"]'),
        list_station: null,         // Список станций
        select_station: station,      // Выбранная станция
        // Инициализировать объект
        initObject: function (select) {
            getAsyncStations(function (result) {
                rw_stations.list_station = result;
                if (select) {
                    rw_stations.initHtmlSelect(result);
                }
            });
        },
        // Инициализировать html компонент select
        initHtmlSelect: function (data) {
            initSelect(
                rw_stations.html_select,
                { width: 300 },
                data,
                function (row) {
                    if (row.view == 1 & row.station_uz == 0) {
                        return { value: row.id, text: (lang == 'en' ? row.name_en : row.name_ru) };
                    }
                },
                -1,
                function (event, ui) {
                    event.preventDefault();
                    var id = Number(ui.item.value);
                    // Должны выбрать станцию
                    if (id > 0) {
                        rw_stations.setSelectStation(id);
                        // Показывать группу пути по умолчанию
                        //$("#group-list").accordion({ active: 0 });
                        //viewGroup(group_list.active, station.id_rc, false);
                    }
                },
            null);
        },
        setSelectStation: function (id) {
            var station = getObjects(rw_stations.list_station, 'id', id)
            if (station != null && station.length > 0) {
                rw_stations.select_station.id = id;
                rw_stations.select_station.name = lang == 'en' ? station[0].name_en : station[0].name_ru;
                rw_stations.select_station.view = station[0].view;
                rw_stations.select_station.exit_uz = station[0].exit_uz;
                rw_stations.select_station.station_uz = station[0].station_uz;
                rw_stations.select_station.id_rs = station[0].id_rs;
                rw_stations.select_station.id_kis = station[0].id_kis;
                rw_stations.select_station.default_side = station[0].default_side;
                rw_stations.select_station.code_uz = station[0].code_uz;
                // Определим станции УЗ прибытия
                var arrival_uz = [];
                var nodes = station[0].StationsNodes1;
                if (nodes != null && nodes.length > 0) {
                    for (i = 0; i < nodes.length; i++) {
                        var station = getObjects(rw_stations.list_station, 'id', nodes[i].id_station_from)
                        if (station != null & station[0].station_uz) {
                            arrival_uz.push({
                                id: station[0].id
                            })
                        }
                    }
                }
                rw_stations.select_station.arrival_uz = arrival_uz;
                // определим горловину по умолчанию
                rw_side.setSideOfStation(rw_stations.select_station.default_side);
                // Показать панели групп
                tab_group_list.activeOfStation(rw_stations.select_station);
                // При выборе новой станции очистить данные по таблицам
                table_cars_details.clearListSelect();
                table_ways.clearListSelect();
                table_arrival_uz.clearListSelect();

                //getAsyncShopsOfStation(station.id_rc, function (result_shop) {
                //    station.list_shops = result_shop;
                //    getAsyncWagonOverturnsOfStation(station.id_rc, function (result_wo) {
                //        station.list_wo = result_wo;
                //        group_list.enableGroup(station.exit_uz, result_shop.length, result_wo.length);
                //    });
                //});
                //panel.info.viewInfoText('Выбрана станция :' + station.name);
                //ways.clearListSelect();
                //wagonoverturns.clearListSelect();
                //shops.clearListSelect();
                //arrival_amkr.clearListSelect();
                //sending.clearListSelect();
                //!!!
            }
        }
    },
    // class side
    side = {
        id: 0,
        side: null,
    },
    //------------------------------------
    //Горловина станции
    rw_side = {
        select_side: side,
        out_default: 0,
        html_select: $('select[name ="side"]'),
        list_side: null,
        // Инициализировать объект
        initObject: function (select) {
            //Загрузим горловину
            getAsyncSide(function (result) {
                rw_side.list_side = result;
                if (select) {
                    rw_side.initHtmlSelect(result);
                }
            });
        },
        // Инициализировать html компонент select
        initHtmlSelect: function (data) {
            initSelect(
                rw_side.html_select,
                { width: 150 },
                data,
                function (row) {
                    return { value: row.value, text: resurses.getText(row.text) };
                },
                rw_side.select_side.id,
                function (event, ui) {
                    event.preventDefault();
                    var id = Number(ui.item.value);
                    if (id >= 0) {
                        rw_side.setSelectSide(id);
                    }
                },
                null);
        },
        // Выбрана горловина 
        setSelectSide: function (id) {
            var side = getObjects(rw_side.list_side, 'value', id)
            if (side != null && side.length > 0) {
                rw_side.select_side.id = side[0].value;
                rw_side.select_side.side = side[0].text;
                // Перестроить позицию вагонов
                table_cars_details.sortPosition();
            }
        },
        // Определить горловину выхода при выборе станции
        setSideOfStation: function (out_default) {
            rw_side.out_default = 1; // Если в таблице null будет определенно как 1
            //rw_side.select = 0;
            if (out_default == true) {
                rw_side.out_default = 1;
                rw_side.setSelectSide(0);
                //rw_side.select = 0
            }
            if (out_default == false) {
                rw_side.out_default = 0;
                rw_side.setSelectSide(1);
                //rw_side.select = 1
            }
            rw_side.html_select.val(rw_side.select_side.id).selectmenu("refresh");
        },

    }

    //-----------------------------------------------------------------------------------------
    // Функции
    //-----------------------------------------------------------------------------------------


    //-----------------------------------------------------------------------------------------
    // Инициализация объектов
    //-----------------------------------------------------------------------------------------

    resurses.initObject("/railway/Scripts/RW/awas.json",
        function () {
            // Загружаем дальше
            tab_group_list.initObject();    // Панель переключения групп
            table_cars_details.initTable(); // таблица информация по вагонам детально
            table_ways.initTable();         // таблица вагоны на путях станций
            table_arrival_uz.initTable();   // таблица прибытия на станции УЗ

            rw_stations.initObject(true);       // Выбор станций системы RailWay
            rw_side.initObject(true);           // Выбор горловины станций системы RailWay


        }); // локализация
});

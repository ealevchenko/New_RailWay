$(function () {
    //$(document).ready(function () {});
    //-----------------------------------------------------------------------------------------
    // Объявление глобальных переменных
    //-----------------------------------------------------------------------------------------
    var lang = $.cookie('lang'),
        panel_detali = $('#detali-group'), // Панель отображения данных по группам
        panel = {
            mode: {
                active: null,
                obj: $('<div class="dt-buttons setup-operation" id="property_view_mode"></div>'),
                selectToggle: function (e) {
                    var target = $(e.target);
                    if (target.is("#mode-view")) {
                        panel.mode.activeView();
                    }
                    if (target.is("#mode-manevr")) {
                        panel.mode.activeManevr();
                    }
                    if (target.is("#mode-sending")) {
                        panel.mode.activeSending();
                    }
                    if (target.is("#mode-correction")) {
                        panel.mode.activeCorrection();
                    }
                    if (target.is("#mode-statepark")) {
                        panel.mode.activeStatepark();
                    }
                    if (target.is("#mode-accept")) {
                        panel.mode.activeAccept();
                    }
                    if (target.is("#mode-transit")) {
                        panel.mode.activeTransit();
                    }
                },
                initPanel: function (view, manevr, sending, accept, transit, correction, statepark) {
                    this.obj.empty();
                    //this.obj.append('<legend>Select a Location: </legend>')
                    if (view) {
                        this.obj.append(panel.element.label_mode_view).append(panel.element.radio_mode_view);
                    };
                    if (manevr) {
                        this.obj.append(panel.element.label_mode_manevr).append(panel.element.radio_mode_manevr);
                    };
                    if (sending) {
                        this.obj.append(panel.element.label_mode_sending).append(panel.element.radio_mode_sending);
                    };
                    if (accept) {
                        this.obj.append(panel.element.label_mode_accept).append(panel.element.radio_mode_accept);
                    };
                    if (transit) {
                        this.obj.append(panel.element.label_mode_transit).append(panel.element.radio_mode_transit);
                    };
                    if (correction) {
                        this.obj.append(panel.element.label_mode_correction).append(panel.element.radio_mode_correction);
                    };
                    if (statepark) {
                        this.obj.append(panel.element.label_mode_statepark).append(panel.element.radio_mode_statepark);
                    };
                    this.obj.controlgroup();
                    // Выберем режим просмотра
                    $("#mode-view").prop('checked', true);
                    panel.mode.activeView();
                    // определим событие выбора режима
                    $("[name='mode']").on("change", panel.mode.selectToggle);
                    panel.mode.obj.controlgroup("refresh"); // Обновим 
                },
                activeView: function () {
                    panel.mode.active = 0;
                    panel.manevr.obj.hide();
                    panel.sent.obj.hide();
                    panel.arrival.obj.hide();
                    panel.accept.obj.hide();
                    cars.clearSelect();
                    panel_detali.removeClass();
                },
                activeManevr: function () {
                    panel.mode.active = 1;
                    panel.manevr.obj.show(); panel.manevr.initPanel();
                    panel.sent.obj.hide();
                    panel.arrival.obj.hide();
                    panel.accept.obj.hide();
                    //panel.transit.obj.hide();
                    cars.clearSelect();
                    panel_detali.removeClass();
                    panel_detali.addClass('mode-manevr');
                },
                activeSending: function () {
                    panel.mode.active = 2;
                    panel.manevr.obj.hide();
                    panel.sent.obj.show(); panel.sent.initPanel();
                    panel.arrival.obj.hide();
                    panel.accept.obj.hide();
                    //panel.transit.obj.hide();
                    cars.clearSelect();
                    panel_detali.removeClass();
                    panel_detali.addClass('mode-sending');
                },
                activeAccept: function () {
                    panel.mode.active = 3;
                    panel.manevr.obj.hide();
                    panel.sent.obj.hide();
                    panel.arrival.obj.hide();
                    panel.accept.obj.show(); panel.accept.initPanel();
                    //panel.transit.obj.hide();
                    cars.clearSelect();
                    panel_detali.removeClass();
                    panel_detali.addClass('mode-accept');
                },
                activeTransit: function () {
                    panel.mode.active = 4;
                    panel.manevr.obj.hide();
                    panel.sent.obj.hide();
                    panel.arrival.obj.hide();
                    panel.accept.obj.show(); panel.accept.initPanel();
                    //panel.transit.obj.show(); panel.transit.initPanel();
                    cars.clearSelect();
                    panel_detali.removeClass();
                    panel_detali.addClass('mode-transit');
                },
                activeCorrection: function () {
                    panel.mode.active = 5;
                    panel.manevr.obj.hide();
                    panel.sent.obj.hide();
                    panel.arrival.obj.hide();
                    panel.accept.obj.hide();
                    //panel.transit.obj.hide();
                    cars.clearSelect();
                    panel_detali.removeClass();
                    panel_detali.addClass('mode-correction');
                },
                activeStatepark: function () {
                    panel.mode.active = 6;
                    panel.manevr.obj.hide();
                    panel.sent.obj.hide();
                    panel.arrival.obj.hide();
                    panel.accept.obj.hide();
                    //panel.transit.obj.hide();
                    cars.clearSelect();
                    panel_detali.removeClass();
                    panel_detali.addClass('mode-statepark');
                },
            },
            view: {
                obj: $('<div class="dt-buttons setup-operation" id="property_view_cars"></div>'),
                handleToggle: function (e) {
                    var target = $(e.target);
                    if (target.is("#view-mt")) {
                        cars.enableColumsMT(target[0].checked);
                    }
                    if (target.is("#view-sap")) {
                        cars.enableColumsSAP(target[0].checked);
                    }
                    if (target.is("#view-email")) {
                        cars.enableColumsEMAIL(target[0].checked);
                    }
                    if (target.is("#view-outcars")) {
                        cars.enableColumsOutCars(target[0].checked);
                    }
                },

                initPanel: function () {
                    this.obj.empty();
                    this.obj.append(panel.element.label_view_mt).append(panel.element.checkbox_view_mt)
                            .append(panel.element.label_view_sap).append(panel.element.checkbox_view_sap)
                            .append(panel.element.label_view_email).append(panel.element.checkbox_view_email)
                            .append(panel.element.label_view_outcars).append(panel.element.checkbox_view_outcars)
                    ;
                    this.obj.controlgroup();
                    panel.element.checkbox_view_mt.on("change", panel.view.handleToggle);
                    panel.element.checkbox_view_sap.on("change", panel.view.handleToggle);
                    panel.element.checkbox_view_email.on("change", panel.view.handleToggle);
                    panel.element.checkbox_view_outcars.on("change", panel.view.handleToggle);
                }
            },
            info: {
                obj: $('<div class="dt-buttons setup-operation" id="property_info"></div>'),
                initPanel: function () {
                    this.obj.append(panel.element.label_info);
                },
                viewInfo: function (obj_select) {
                    switch (obj_select.name) {
                        case 'list_ways':
                            if (obj_select.select_obj != null) {
                                panel.info.viewInfoText('Выбран путь : ' + obj_select.select_obj.num + ' - ' + obj_select.select_obj.name);
                            } else {
                                panel.info.viewInfoText('Путь не выбран');
                            };
                            break;
                        case 'list_wagonoverturns':
                            if (obj_select.select_obj != null) {
                                panel.info.viewInfoText('Выбран вагоноопрокид : ' + obj_select.select_obj.name);
                            } else {
                                panel.info.viewInfoText('Вагоноопрокид не выбран');
                            };
                            break;
                        case 'list_shops':
                            if (obj_select.select_obj != null) {
                                panel.info.viewInfoText('Выбран цех : ' + obj_select.select_obj.name);
                            } else {
                                panel.info.viewInfoText('Цех не выбран');
                            };
                            break;
                        case 'list_arrival_amkr':
                            if (obj_select.select_obj != null) {
                                panel.info.viewInfoText('Выбран прибывающий состав, станция отправления :' + obj_select.select_obj.stat
                                    + ', № поезда :' + obj_select.select_obj.st_lock_train
                                    + ', Дата и время отправления :' + obj_select.select_obj.dt_from_stat);
                            } else {
                                panel.info.viewInfoText('Состав не выбран');
                            };
                            break;
                        case 'list_sending':
                            if (obj_select.select_obj != null) {
                                panel.info.viewInfoText('Выбран отправленный ранее состав, станция назначения :' + obj_select.select_obj.stat
                                    + ', № поезда :' + obj_select.select_obj.st_lock_train
                                    + ', Дата и время отправления :' + obj_select.select_obj.dt_from_stat);
                            } else {
                                panel.info.viewInfoText('Состав не выбран');
                            };
                            break;
                        default:
                            panel.info.viewInfoText('');
                            break;
                    }
                },
                viewInfoText: function (value) {
                    panel.element.label_info.text(value);
                },
            },
            manevr: {
                obj: $('<div class="dt-buttons setup-operation" id="property_manevr_operation"></div>'),
                initPanel: function () {
                    this.obj.empty();
                    panel.element.button_manevr_select_all.on('click', function () {
                        cars.allSelect();
                    });
                    panel.element.button_manevr_clear_all.on('click', function () {
                        cars.clearSelect();
                    });
                    this.obj.append(panel.element.button_manevr_select_all)
                            .append(panel.element.button_manevr_clear_all)
                            .append(panel.element.label_gorlov_manevr)
                            .append(panel.element.select_manevr_side)
                            .append(panel.element.label_way_manevr)
                            .append(panel.element.select_manevr_way)
                            .append(panel.element.button_ok_manevr);
                    initSelect(panel.element.select_manevr_side, { width: 150 }, side.list, null, 0, function (event, ui) { event.preventDefault(); }, null);
                    initSelect(panel.element.select_manevr_way, { width: 300 }, ways.list, function (row) { return { value: row.id_way, text: row.num + '-' + row.name }; }, -1, function (event, ui) { event.preventDefault(); }, null);

                },
            },
            sent: {
                obj: $('<div class="dt-buttons setup-operation" id="property_sent_operation"></div>'),
                initPanel: function () {
                    this.obj.empty();
                    panel.element.button_sent_select_all.on('click', function () {
                        cars.allSelect();
                    });
                    panel.element.button_sent_clear_all.on('click', function () {
                        cars.clearSelect();
                    });
                    this.obj.append(panel.element.button_sent_select_all)
                            .append(panel.element.button_sent_clear_all)
                            .append(panel.element.label_sent_station)
                            .append(panel.element.select_sent_station);
                    initSelect(panel.element.select_sent_station, { width: 300 }, nodes.getListSend(station), function (row) {
                        var stat = getObjects(station.list, 'id', row.id_station_on);
                        return { value: stat[0].id, text: (lang == 'en' ? stat[0].name_en : stat[0].name_ru) };
                    }, -1, function (event, ui) { event.preventDefault(); }, null);
                    if (station.list_wo.length > 0) {
                        this.obj.append(panel.element.label_sent_wo)
                        .append(panel.element.select_sent_wo);
                        initSelect(panel.element.select_sent_wo, { width: 200 }, station.list_wo, function (row) { return { value: row.id_gruz_front, text: row.name }; }, -1, function (event, ui) { event.preventDefault(); }, null);
                    }
                    if (station.list_shops.length > 0) {
                        this.obj.append(panel.element.label_sent_shop)
                        .append(panel.element.select_sent_shop);
                        initSelect(panel.element.select_sent_shop, { width: 200 }, station.list_shops, function (row) { return { value: row.id_shop, text: row.name }; }, -1, function (event, ui) { event.preventDefault(); }, null);
                    }

                    this.obj.append(panel.element.label_sent_side)
                    .append(panel.element.select_sent_side)
                    .append(panel.element.button_ok_sent);
                    initSelect(panel.element.select_sent_side, { width: 150 }, side.list, null, 0, function (event, ui) { event.preventDefault(); }, null);


                    //nodes.getListSend(station);
                },
            },
            arrival: {
                obj: $('<div class="dt-buttons setup-operation" id="property_arrival_operation"></div>'),
                initPanel: function () {
                    //this.obj.append(panel.element.button_sent_select_all)
                    //        .append(panel.element.button_sent_clear_all)
                    //        .append(panel.element.label_sent_station)
                    //        .append(panel.element.select_sent_station)
                    //        .append(panel.element.label_sent_wo)
                    //        .append(panel.element.select_sent_wo)
                    //        .append(panel.element.label_sent_shop)
                    //        .append(panel.element.select_sent_shop)
                    //        .append(panel.element.label_sent_side)
                    //        .append(panel.element.select_sent_side)
                    //        .append(panel.element.button_ok_sent);
                },
            },
            accept: {
                obj: $('<div class="dt-buttons setup-operation" id="property_accept_operation"></div>'),
                initPanel: function () {
                    this.obj.empty();
                    panel.element.button_accept_select_all.on('click', function () {
                        cars.allSelect();
                    });
                    panel.element.button_accept_clear_all.on('click', function () {
                        cars.clearSelect();
                    });
                    this.obj.append(panel.element.button_accept_select_all)
                            .append(panel.element.button_accept_clear_all)
                            .append(panel.mode.active == 3 ? panel.element.label_accept_way : panel.element.label_transit_way)
                            .append(panel.element.select_accept_way);
                    initSelect(panel.element.select_accept_way, { width: 300 }, ways.list, function (row) { return { value: row.id_way, text: row.num + '-' + row.name }; }, -1, function (event, ui) { event.preventDefault(); }, null);
                    if (group_list.active != 1 && group_list.active != 2 && panel.mode.active == 3) {
                        this.obj.append(panel.element.label_accept_side)
                                .append(panel.element.select_accept_side);
                        initSelect(panel.element.select_accept_side, { width: 150 }, side.list, null, 0, function (event, ui) { event.preventDefault(); }, null);
                    }
                    if (panel.mode.active == 4) {
                        this.obj.append(panel.element.label_transit_station)
                                .append(panel.element.select_transit_station);
                        initSelect(panel.element.select_transit_station, { width: 300 }, nodes.getListSend(station), function (row) {
                            var stat = getObjects(station.list, 'id', row.id_station_on);
                            return { value: stat[0].id, text: (lang == 'en' ? stat[0].name_en : stat[0].name_ru) };
                        }, -1, function (event, ui) { event.preventDefault(); }, null);
                    }
                    this.obj.append(panel.element.label_accept_datetime)
                            .append(panel.element.input_accept_side)
                            .append(panel.element.button_ok_accept);
                    initDateTime($('#accept-datetime'), null);
                },
            },
            element: {
                // панель режимов
                label_mode_view: $('<label for="mode-view">' + (lang == 'en' ? "View" : "Просмотр") + '</label>'),
                radio_mode_view: $('<input type="radio" name="mode" id="mode-view" checked="checked" >'),
                label_mode_manevr: $('<label for="mode-manevr">' + (lang == 'en' ? "Maneuvers " : "Маневры") + '</label>'),
                radio_mode_manevr: $('<input type="radio" name="mode" id="mode-manevr">'),
                label_mode_sending: $('<label for="mode-sending">' + (lang == 'en' ? "Sending " : "Отправка") + '</label>'),
                radio_mode_sending: $('<input type="radio" name="mode" id="mode-sending">'),
                label_mode_correction: $('<label for="mode-correction">' + (lang == 'en' ? "Correction " : "Коррекция") + '</label>'),
                radio_mode_correction: $('<input type="radio" name="mode" id="mode-correction">'),
                label_mode_statepark: $('<label for="mode-statepark">' + (lang == 'en' ? "State of the park " : "Состояние парка") + '</label>'),
                radio_mode_statepark: $('<input type="radio" name="mode" id="mode-statepark">'),
                label_mode_accept: $('<label for="mode-accept">' + (lang == 'en' ? "To accept " : "Принять ") + '</label>'),
                radio_mode_accept: $('<input type="radio" name="mode" id="mode-accept">'),
                label_mode_transit: $('<label for="mode-transit">' + (lang == 'en' ? "Transit " : "Транзит ") + '</label>'),
                radio_mode_transit: $('<input type="radio" name="mode" id="mode-transit">'),
                // панель просмотра
                label_info: $('<label id="view-info">Информация:</label>'),
                label_view_mt: $('<label for="view-mt">' + (lang == 'en' ? "MetallurgTrans" : "МеталургТранс") + '</label>'),
                checkbox_view_mt: $('<input type="checkbox" name="view" id="view-mt" checked="checked" >'),
                label_view_sap: $('<label for="view-sap">' + (lang == 'en' ? "SAP" : "САП") + '</label>'),
                checkbox_view_sap: $('<input type="checkbox" name="view" id="view-sap" checked="checked" >'),
                label_view_email: $('<label for="view-email">' + (lang == 'en' ? "Writing" : "Письма") + '</label>'),
                checkbox_view_email: $('<input type="checkbox" name="view" id="view-email" checked="checked" >'),
                label_view_outcars: $('<label for="view-outcars">' + (lang == 'en' ? "Shipped goods" : "Отправляемые грузы") + '</label>'),
                checkbox_view_outcars: $('<input type="checkbox" name="view" id="view-outcars" checked="checked" >'),
                // панель настроек маневра
                button_manevr_select_all: $('<button class="ui-button ui-widget ui-corner-all">' + (lang == 'en' ? "Select All" : "Выбрать все вагоны") + '</button>'),
                button_manevr_clear_all: $('<button class="ui-button ui-widget ui-corner-all">' + (lang == 'en' ? "Clear All" : "Убрать все вагоны") + '</button>'),
                label_gorlov_manevr: $('<label class="setup-label">' + (lang == 'en' ? "The throat of maneuver:" : "Горловина маневра:") + '</label>'),
                label_way_manevr: $('<label class="setup-label">' + (lang == 'en' ? "Maneuver on the way:" : "Маневр на путь:") + '</label>'),
                select_manevr_side: $('<select id="manevr-side" name="station"></select>'),
                select_manevr_way: $('<select id="manevr-way" name="station"></select>'),
                button_ok_manevr: $('<button class="ui-button ui-widget ui-corner-all">' + (lang == 'en' ? "Perform a maneuver" : "Выполнить маневр") + '</button>'),
                // панель настроек отправки на другие станции
                button_sent_select_all: $('<button class="ui-button ui-widget ui-corner-all">' + (lang == 'en' ? "Select All" : "Выбрать все вагоны") + '</button>'),
                button_sent_clear_all: $('<button class="ui-button ui-widget ui-corner-all">' + (lang == 'en' ? "Clear All" : "Убрать все вагоны") + '</button>'),
                label_sent_station: $('<label class="setup-label" for="sent-station">' + (lang == 'en' ? "Receiving station:" : "Станция приема:") + '</label>'),
                select_sent_station: $('<select id="sent-station" name="sent-station"></select>'),
                label_sent_wo: $('<label class="setup-label" for="sent-wo">' + (lang == 'en' ? "Wagon overturns:" : "Вагоноопрокид:") + '</label>'),
                select_sent_wo: $('<select id="sent-wo" name="sent-wo"></select>'),
                label_sent_shop: $('<label class="setup-label" for="sent-shop">' + (lang == 'en' ? "Shops:" : "Цех:") + '</label>'),
                select_sent_shop: $('<select id="sent-shop" name="sent-shop"></select>'),
                label_sent_side: $('<label class="setup-label" for="sent-side">' + (lang == 'en' ? "Sending neck:" : "Горловина отправки:") + '</label>'),
                select_sent_side: $('<select id="sent-side" name="sent-side"></select>'),
                button_ok_sent: $('<button class="ui-button ui-widget ui-corner-all">' + (lang == 'en' ? "Send and" : "Выполнить отправку") + '</button>'),
                // панель настроек принятия из других станции
                button_accept_select_all: $('<button class="ui-button ui-widget ui-corner-all">' + (lang == 'en' ? "Select All" : "Выбрать все вагоны") + '</button>'),
                button_accept_clear_all: $('<button class="ui-button ui-widget ui-corner-all">' + (lang == 'en' ? "Clear All" : "Убрать все вагоны") + '</button>'),
                label_accept_way: $('<label class="setup-label" for="accept-way">' + (lang == 'en' ? "Take the path:" : "Принять на путь:") + '</label>'),
                label_transit_way: $('<label class="setup-label" for="accept-way">' + (lang == 'en' ? "The transit route:" : "Путь транзита:") + '</label>'),
                select_accept_way: $('<select id="accept-way" name="accept-way"></select>'),
                label_accept_side: $('<label class="setup-label" for="accept-side">' + (lang == 'en' ? "Reception from:" : "Прием со стороны горл.:") + '</label>'),
                select_accept_side: $('<select id="accept-side" name="accept-side"></select>'),
                label_accept_datetime: $('<label class="setup-label" for="accept-datetime">' + (lang == 'en' ? "Date and Time:" : "Дата и время:") + '</label>'),
                input_accept_side: $('<input id="accept-datetime" name="accept-datetime" type="datetime">'),
                label_transit_station: $('<label class="setup-label" for="transit-station">' + (lang == 'en' ? "Send to the station:" : "Отправить на станцию:") + '</label>'),
                select_transit_station: $('<select id="transit-station" name="transit-station"></select>'),
                button_ok_accept: $('<button class="ui-button ui-widget ui-corner-all">' + (lang == 'en' ? "Execute" : "Выполнить") + '</button>'),
            },
            initPanel: function (obj) {
                // Всегда показывать
                panel.view.initPanel();
                panel.info.initPanel();
                obj.prepend(panel.accept.obj);
                obj.prepend(panel.sent.obj);
                obj.prepend(panel.manevr.obj);
                obj.prepend(panel.info.obj);
                obj.prepend(panel.view.obj);
                obj.prepend(panel.mode.obj);
                //obj.prepend(panel.transit.obj);
            },
            //активировать панель 
            activePanel: function (active_group) {
                panel.view.obj.show();
                panel.mode.obj.hide();
                panel.manevr.obj.hide();
                panel.sent.obj.hide();
                panel.arrival.obj.hide();
                panel.accept.obj.hide();
                //panel.transit.obj.hide();
                switch (active_group) {
                    case 0:
                        panel.mode.obj.show();
                        panel.mode.initPanel(true, true, true, false, false, true, true);
                        break;
                    case 1:
                        panel.mode.obj.show();
                        panel.mode.initPanel(true, false, false, true, true, true, true);
                        break;
                    case 2:
                        panel.mode.obj.show();
                        panel.mode.initPanel(true, false, false, true, true, true, true);
                        break;
                    case 3:
                        panel.mode.obj.show();
                        panel.mode.initPanel(true, false, false, true, true, false, false);
                        break;
                    case 4:
                        panel.mode.obj.show();
                        panel.mode.initPanel(true, false, false, true, false, false, false);
                        break;
                    case 5:
                        panel.mode.obj.hide();
                        break;
                    default:
                        // Группы закрыты

                        break;
                }

            }
        },
        //Панель групп списков
        group_list = {
            active: 0,
            obj: null,
            station: null,
            list_ways: $('#group-list-ways'),
            list_wagonoverturns: $('#group-list-wagonoverturns'),    // Группа элементов отображения информации о вагоноопрокидах
            list_shops: $('#group-list-shops'),
            list_arrival: $('#group-list-arrival-amkr'),
            list_arrivaluz: $('#group-list-arrival-uz'),
            list_sending: $('#group-list-sending'),
            initGroup: function () {
                this.obj = $("#group-list").accordion({
                    collapsible: true,
                    heightStyle: "content",
                    activate: function (event, ui) {
                        group_list.active = group_list.obj.accordion("option", "active");
                        viewGroup(group_list.active, station.id_rc, false)
                    },
                });
                this.list_ways.hide();
                this.list_wagonoverturns.hide();    // Группа элементов отображения информации о вагоноопрокидах
                this.list_shops.hide();
                this.list_arrival.hide();
                this.list_arrivaluz.hide();
                this.list_sending.hide();
            },
            enableGroup: function (exit_uz, count_shops, count_wo) {
                this.list_ways.show();
                if (count_wo > 0) { this.list_wagonoverturns.show(); } else { this.list_wagonoverturns.hide(); }  // Группа элементов отображения информации о вагоноопрокидах
                if (count_shops > 0) { this.list_shops.show(); } else { this.list_shops.hide(); }
                this.list_arrival.show();
                if (exit_uz) { this.list_arrivaluz.show(); } else { this.list_arrivaluz.hide(); }
                this.list_sending.show();
            }
        },
        //Горловина станции
        side = {
            select: 0,
            out_default: 0,
            obj: $('select[name ="side"]'),
            list: null,
            // Определить горловину выхода при выборе станции
            setSideOfStation: function (out_default) {
                side.out_default = 1; // Если в таблице null будет определенно как 1
                side.select = 0;
                if (out_default == true) {
                    side.out_default = 1;
                    side.select = 0
                }
                if (out_default == false) {
                    side.out_default = 0;
                    side.select = 1
                }
                side.obj.val(side.select).selectmenu("refresh");
            }
        },
        //Выбранная станция
        station = {
            select: -1,
            id_rc: -1,
            name: null,
            exit_uz: false,
            obj: $('select[name ="station"]'),
            list: null,
            list_wo: null,
            list_shops: null,
            // Получить новые свойства выбранной станции
            setStationProperty: function (id) {
                station.select = id;
                var stat = getObjects(station.list, 'id', station.select)
                if (stat != null && stat.length > 0) {
                    station.id_rc = stat[0].id_rs;
                    station.name = lang == 'en' ? stat[0].name_en : stat[0].name_ru;
                    station.exit_uz = stat[0].exit_uz;
                    // определим горловину по умолчанию
                    side.setSideOfStation(stat[0].default_side);
                    // Показать панели групп
                    getAsyncShopsOfStation(station.id_rc, function (result_shop) {
                        station.list_shops = result_shop;
                        getAsyncWagonOverturnsOfStation(station.id_rc, function (result_wo) {
                            station.list_wo = result_wo;
                            group_list.enableGroup(station.exit_uz, result_shop.length, result_wo.length);
                        });
                    });
                    panel.info.viewInfoText('Выбрана станция :' + station.name);
                    ways.clearListSelect();
                    wagonoverturns.clearListSelect();
                    shops.clearListSelect();
                    arrival_amkr.clearListSelect();
                    sending.clearListSelect();
                    //!!!
                }
            },
            initStation: function (select) {
                getAsyncStations(function (result) {
                    station.list = result;
                    if (select) {
                        station.initSelect(result);
                    }
                });
            },

            initSelect: function (data) {
                initSelect(
                    station.obj,
                    { width: 300 },
                    data,
                    function (row) {
                        if (row.view == 1 & row.station_uz == 0) {
                            return { value: row.id, text: (lang == 'en' ? row.name_en : row.name_ru) };
                        }
                    },
                station.select,
                function (event, ui) {
                    event.preventDefault();
                    var id = Number(ui.item.value);
                    // Должны выбрать станцию
                    if (id > 0) {
                        station.setStationProperty(id);
                        // Показывать группу пути по умолчанию
                        $("#group-list").accordion({ active: 0 });
                        viewGroup(group_list.active, station.id_rc, false);
                    }
                },
                null);
            }
        },
        nodes = {
            list: null,
            initNodes: function () {
                getAsyncStationsNodes(function (result) { nodes.list = result; });
            },
            getListSend: function (station) {
                return getObjects(nodes.list, 'id_station_from', station.select)
            }
        },
        // Группа списков путей станции
        ways = {
            name: 'list_ways',
            station_id: null,
            select: null,
            select_obj: null,
            table: null,
            obj_table: null,
            obj: null,
            list: null,
            initTable: function () {
                this.table = $('#table-list-ways');
                this.obj = ways.table.DataTable({
                    "paging": false,
                    "ordering": false,
                    "info": false,
                    "select": false,
                    "filter": false,
                    //"scrollY": "550px",
                    language: {
                        emptyTable: lang == 'en' ? "No data available in table" : "Данные отсутствуют",
                    },
                    jQueryUI: true,
                    "createdRow": function (row, data, index) {
                        $(row).attr('id', data.id_way);
                        if (data.id_way == ways.select) {
                            $(row).addClass('selected');
                            //viewCars(way_select_id, wagonoverturns_select_id, shops_select_id, side_station, default_side);
                        }
                    },
                    columns: [
                        { data: "num", title: "№", width: "30px" },
                        { data: "name", title: "Путь" },
                        { data: "vag_amount", title: "Кол. ваг.", width: "30px" },
                        { data: "vag_capacity", title: "Вмест. ваг.", width: "30px" },
                    ],
                });
                this.obj_table = $('DIV#table-list-ways_wrapper');
                this.initEventSelect();
                this.obj_table.hide();
            },
            initEventSelect: function () {
                this.table.find('tbody')
                        .on('click', 'tr', function () {
                            ways.clearSelect();
                            $(this).addClass('selected');
                            ways.select = $(this).attr("id");
                            ways.getSelectObj(ways.select);
                            cars.viewTable(ways, false);
                        });
            },
            getSelectObj: function (select) {
                var obj = getObjects(ways.list, 'id_way', select);
                if (obj != null) {
                    ways.select_obj = obj[0];
                }
            },
            clearSelect: function () {
                this.table.find('tbody tr').removeClass('selected');
            },
            loadData: function (data) {
                this.list = data;
                this.obj.clear();
                for (i = 0; i < data.length; i++) {
                    this.obj.row.add({
                        "id_way": data[i].id_way,
                        "num": data[i].num,
                        "name": data[i].name,
                        "vag_amount": data[i].vag_amount,
                        "vag_capacity": data[i].vag_capacity,
                    });
                }
                this.obj.draw();
            },
            viewTable: function (station_id, data_refresh) {
                this.obj_table.show();
                if (this.list == null | this.station_id != station_id | data_refresh == true) {
                    // Обновим данные
                    this.station_id = station_id;
                    getAsyncWaysStation(
                        station_id,
                        false,
                        function (result) {
                            ways.loadData(result);
                            cars.viewTable(ways, false);
                        }
                        );
                } else {
                    // Не обновим данные
                    cars.viewTable(ways, false);
                }
            },
            clearListSelect: function () {
                this.list = null;
                this.select = null;
                this.select_obj = null;
            }
        },
        wagonoverturns = {
            name: 'list_wagonoverturns',
            station_id: null,
            select: null,
            select_obj: null,
            table: null,
            obj_table: null,
            obj: null,
            list: null,
            initTable: function () {
                this.table = $('#table-list-wagonoverturns');
                this.obj = this.table.DataTable({
                    "paging": false,
                    "ordering": false,
                    "info": false,
                    "select": false,
                    "filter": false,
                    //"scrollY": "550px",
                    language: {
                        emptyTable: lang == 'en' ? "No data available in table" : "Данные отсутствуют",
                    },
                    jQueryUI: true,
                    "createdRow": function (row, data, index) {
                        $(row).attr('id', data.id_gruz_front);
                        if (data.id_gruz_front == this.select) {
                            $(row).addClass('selected');

                        }
                    },
                    columns: [
                        { data: "name", title: "Путь" },
                        { data: "vag_amount", title: "Кол. ваг.", width: "30px" },
                    ],
                });
                this.obj_table = $('DIV#table-list-wagonoverturns_wrapper');
                this.initEventSelect();
                this.obj_table.hide();
            },
            initEventSelect: function () {
                this.table.find('tbody')
                        .on('click', 'tr', function () {
                            wagonoverturns.clearSelect();
                            $(this).addClass('selected');
                            wagonoverturns.select = $(this).attr("id");
                            wagonoverturns.getSelectObj(wagonoverturns.select);
                            cars.viewTable(wagonoverturns, false);
                        });
            },
            getSelectObj: function (select) {
                var obj = getObjects(wagonoverturns.list, 'id_gruz_front', select);
                if (obj != null) {
                    wagonoverturns.select_obj = obj[0];
                }
            },
            clearSelect: function () {
                this.table.find('tbody tr').removeClass('selected');
            },
            loadData: function (data) {
                this.list = data;
                this.obj.clear();
                for (i = 0; i < data.length; i++) {
                    this.obj.row.add({
                        "id_gruz_front": data[i].id_gruz_front,
                        "name": data[i].name,
                        "vag_amount": data[i].vag_amount,
                    });
                }
                this.obj.draw();
            },
            enableTable: function (length) {
                if (length > 0) {
                    this.obj_table.show();
                } else {
                    this.obj_table.hide();
                }
            },
            viewTable: function (station_id, data_refresh, callback) {

                if (this.list == null | this.station_id != station_id | data_refresh == true) {
                    // Обновим данные
                    this.station_id = station_id;
                    getAsyncWagonOverturnsStation(
                        station_id,
                        function (result) {
                            wagonoverturns.loadData(result);
                            wagonoverturns.enableTable(result.length);
                            cars.viewTable(wagonoverturns, false);
                            if (typeof callback === 'function') {
                                callback(result.length);
                            }
                        }
                        );
                } else {
                    this.enableTable(this.list.length);
                    cars.viewTable(wagonoverturns, false);
                    if (typeof callback === 'function') {
                        callback(this.list.length);
                    }
                };
            },
            clearListSelect: function () {
                this.list = null;
                this.select = null;
                this.select_obj = null;
            }
        },
        shops = {
            name: 'list_shops',
            station_id: null,
            select: null,
            select_obj: null,
            table: null,
            obj_table: null,
            obj: null,
            list: null,
            initTable: function () {
                this.table = $('#table-list-shops');
                this.obj = this.table.DataTable({
                    "paging": false,
                    "ordering": false,
                    "info": false,
                    "select": false,
                    "filter": false,
                    //"scrollY": "550px",
                    language: {
                        emptyTable: lang == 'en' ? "No data available in table" : "Данные отсутствуют",
                    },
                    jQueryUI: true,
                    "createdRow": function (row, data, index) {
                        $(row).attr('id', data.id_shop);
                        if (data.id_shop == this.select) {
                            $(row).addClass('selected');
                            //viewCars(way_select_id, wagonoverturns_select_id, shops_select_id, side_station, default_side);
                        }
                    },
                    columns: [
                        { data: "name", title: "Путь" },
                        { data: "vag_amount", title: "Кол. ваг.", width: "30px" },
                    ],
                });
                this.obj_table = $('DIV#table-list-shops_wrapper');
                this.initEventSelect();
                this.obj_table.hide();
            },
            initEventSelect: function () {
                this.table.find('tbody')
                        .on('click', 'tr', function () {
                            shops.clearSelect();
                            $(this).addClass('selected');
                            shops.select = $(this).attr("id");
                            shops.getSelectObj(shops.select);
                            cars.viewTable(shops, false);
                        });
            },
            getSelectObj: function (select) {
                var obj = getObjects(shops.list, 'id_shop', select);
                if (obj != null) {
                    shops.select_obj = obj[0];
                }
            },
            clearSelect: function () {
                this.table.find('tbody tr').removeClass('selected');
            },
            loadData: function (data) {
                this.list = data;
                this.obj.clear();
                for (i = 0; i < data.length; i++) {
                    this.obj.row.add({
                        "id_shop": data[i].id_shop,
                        "name": data[i].name,
                        "vag_amount": data[i].vag_amount,
                    });
                }
                this.obj.draw();
            },
            enableTable: function (length) {
                if (length > 0) {
                    this.obj_table.show();
                } else {
                    this.obj_table.hide();
                }
            },
            viewTable: function (station_id, data_refresh, callback) {

                if (this.list == null | this.station_id != station_id | data_refresh == true) {
                    // Обновим данные
                    this.station_id = station_id;
                    getAsyncShopStation(
                        station_id,
                        function (result) {
                            shops.loadData(result);
                            shops.enableTable(result.length);
                            cars.viewTable(shops, false);
                            if (typeof callback === 'function') {
                                callback(result.length);
                            }
                        }
                        );
                } else {
                    this.enableTable(this.list.length);
                    cars.viewTable(shops, false);
                    if (typeof callback === 'function') {
                        callback(this.list.length);
                    }
                };
            },
            clearListSelect: function () {
                this.list = null;
                this.select = null;
                this.select_obj = null;
            }
        },
        arrival_amkr = {
            name: 'list_arrival_amkr',
            station_id: null,
            train: null,
            dt: null,
            select_obj: null,
            table: null,
            obj_table: null,
            obj: null,
            list: null,
            initTable: function () {
                this.table = $('#table-list-arrival-amkr');
                this.obj = this.table.DataTable({
                    "paging": false,
                    "ordering": false,
                    "info": false,
                    "select": false,
                    "filter": false,
                    //"scrollY": "550px",
                    language: {
                        emptyTable: lang == 'en' ? "No data available in table" : "Данные отсутствуют",
                    },
                    jQueryUI: true,
                    "createdRow": function (row, data, index) {
                        $(row).attr('train', data.st_lock_train).attr('dt', data.dt_from_stat);

                        if (data.st_lock_train == this.train) {
                            $(row).addClass('selected');

                        }
                    },
                    columns: [
                        { data: "dt_from_stat", title: "Отправлен" },
                        { data: "stat", title: "Станция" },
                        { data: "vag_amount", title: "Кол. ваг.", width: "30px" },
                    ],
                });
                this.obj_table = $('DIV#table-list-arrival-amkr_wrapper');
                this.initEventSelect();
                this.obj_table.hide();
            },
            initEventSelect: function () {
                this.table.find('tbody')
                        .on('click', 'tr', function () {
                            arrival_amkr.clearSelect();
                            $(this).addClass('selected');
                            arrival_amkr.train = $(this).attr("train");
                            arrival_amkr.dt = $(this).attr("dt");
                            arrival_amkr.getSelectObj(arrival_amkr.train, arrival_amkr.dt);
                            cars.viewTable(arrival_amkr, false);
                        });
            },
            getSelectObj: function (train, dt) {
                var obj = getObjects(arrival_amkr.list, 'st_lock_train', train);
                if (obj != null) {
                    var obj = getObjects(obj, 'dt_from_stat', dt);
                    if (obj != null) {
                        arrival_amkr.select_obj = obj[0];
                    }
                }
            },
            clearSelect: function () {
                this.table.find('tbody tr').removeClass('selected');
            },
            loadData: function (data) {
                this.list = data;
                this.obj.clear();
                for (i = 0; i < data.length; i++) {
                    // Исключим станции Кривого Рога
                    var stat = getObjects(station.list, 'id_rs', data[i].id_stat)
                    if (stat != null && stat.length > 0) {
                        if (stat[0].station_uz == 0) {
                            // Добавим данные о станциях
                            this.obj.row.add({
                                "id_stat": data[i].id_stat,
                                "stat": data[i].stat,
                                "st_lock_train": data[i].st_lock_train,
                                "dt_from_stat": data[i].dt_from_stat,
                                "vag_amount": data[i].vag_amount,
                            });
                        }
                    };
                };
                this.obj.draw();
            },
            enableTable: function (length) {
                if (length > 0) {
                    this.obj_table.show();
                } else {
                    this.obj_table.hide();
                }
            },
            viewTable: function (station_id, data_refresh, callback) {

                if (this.list == null | this.station_id != station_id | data_refresh == true) {
                    // Обновим данные
                    this.station_id = station_id;
                    getAsyncArrivalAMKRStation(
                        station_id,
                        function (result) {
                            arrival_amkr.loadData(result);
                            arrival_amkr.enableTable(result.length);
                            cars.viewTable(arrival_amkr, false);
                            if (typeof callback === 'function') {
                                callback(result.length);
                            }
                        }
                        );
                } else {
                    this.enableTable(this.list.length);
                    cars.viewTable(arrival_amkr, false);
                    if (typeof callback === 'function') {
                        callback(this.list.length);
                    }
                };
            },
            clearListSelect: function () {
                this.list = null;
                this.train = null;
                this.dt = null;
                this.select_obj = null;
            }
        },
        arrival_uz = {
            name: 'list_arrival_uz',
            station_id: null,
            select: null,
            select_obj: null,
            table: null,
            obj_table: null,
            obj: null,
            list: null,
            initTable: function () {
                this.table = $('#table-list-arrival-uz');
                this.obj = this.table.DataTable({
                    "paging": false,
                    "ordering": false,
                    "info": false,
                    "select": false,
                    "filter": false,
                    //"scrollY": "550px",
                    language: {
                        emptyTable: lang == 'en' ? "No data available in table" : "Данные отсутствуют",
                    },
                    jQueryUI: true,
                    "createdRow": function (row, data, index) {
                        $(row).attr('id', data.IDSostav);
                        if (data.IDSostav == this.IDSostav) {
                            $(row).addClass('selected');

                        }
                    },
                    columns: [
                        { data: "CompositionIndex", title: "Состав" },
                        { data: "DateOperation", title: "Дата и время" },
                        { data: "count_cars", title: "Кол. ваг.", width: "30px" },
                    ],
                });
                this.obj_table = $('DIV#table-list-arrival-uz_wrapper');
                this.initEventSelect();
                this.obj_table.hide();
            },
            initEventSelect: function () {
                this.table.find('tbody')
                        .on('click', 'tr', function () {
                            arrival_uz.clearSelect();
                            $(this).addClass('selected');
                            arrival_uz.select = $(this).attr("id");
                            arrival_uz.getSelectObj(arrival_uz.select);
                            cars.viewTable(arrival_uz, false);
                        });
            },
            getSelectObj: function (select) {
                var obj = getObjects(arrival_uz.list, 'IDSostav', select);
                if (obj != null) {
                    arrival_uz.select_obj = obj[0];
                }
            },
            clearSelect: function () {
                this.table.find('tbody tr').removeClass('selected');
            },
            loadData: function (data) {
                this.list = data;
                this.obj.clear();
                for (i = 0; i < data.length; i++) {
                    // Добавим данные о станциях
                    this.obj.row.add({
                        "IDSostav": data[i].IDSostav,
                        "CompositionIndex": data[i].CompositionIndex,
                        "DateOperation": data[i].DateOperation,
                        "count_cars": data[i].count_cars,
                    });
                };
                this.obj.draw();
            },
            enableTable: function (length) {
                if (length > 0) {
                    this.obj_table.show();
                } else {
                    this.obj_table.hide();
                }
            },
            viewTable: function (station_id, data_refresh, callback) {

                if (this.list == null | this.station_id != station_id | data_refresh == true) {
                    // Обновим данные
                    this.station_id = station_id;
                    getNoCloseArrivalCarsOfStationUZ(
                        (station_id == 20 ? 4672 : 4670),
                        function (result) {
                            arrival_uz.loadData(result);
                            arrival_uz.enableTable(result.length);
                            cars.viewTable(arrival_uz, false);
                            if (typeof callback === 'function') {
                                callback(result.length);
                            }
                        }
                        );
                } else {
                    this.enableTable(this.list.length);
                    cars.viewTable(arrival_uz, false);
                    if (typeof callback === 'function') {
                        callback(this.list.length);
                    }
                };
            },
            clearListSelect: function () {
                this.list = null;
                this.select = null;
                this.select_obj = null;
            }
        },
        sending = {
            name: 'list_sending',
            station_id: null,
            train: null,
            dt: null,
            station_to: null,
            select_obj: null,
            table: null,
            obj_table: null,
            obj: null,
            list: null,
            initTable: function () {
                this.table = $('#table-list-sending');
                this.obj = this.table.DataTable({
                    "paging": false,
                    "ordering": false,
                    "info": false,
                    "select": false,
                    "filter": false,
                    //"scrollY": "550px",
                    language: {
                        emptyTable: lang == 'en' ? "No data available in table" : "Данные отсутствуют",
                    },
                    jQueryUI: true,
                    "createdRow": function (row, data, index) {
                        $(row).attr('train', data.st_lock_train).attr('dt', data.dt_from_stat).attr('station-to', data.st_lock_id_stat);
                        if (data.st_lock_train == this.train && data.dt_from_stat == this.dt) {
                            $(row).addClass('selected');

                        }
                    },
                    columns: [
                        { data: "dt_from_stat", title: "Отправлен" },
                        { data: "stat", title: "Станция" },
                        { data: "vag_amount", title: "Кол. ваг.", width: "30px" },
                    ],
                });
                this.obj_table = $('DIV#table-list-sending_wrapper');
                this.initEventSelect();
                this.obj_table.hide();
            },
            initEventSelect: function () {
                this.table.find('tbody')
                        .on('click', 'tr', function () {
                            sending.clearSelect();
                            $(this).addClass('selected');
                            sending.train = $(this).attr("train");
                            sending.dt = $(this).attr("dt");
                            sending.station_to = $(this).attr("station-to");
                            sending.getSelectObj(sending.train, sending.dt, sending.station_to);
                            cars.viewTable(sending, false);
                        });
            },
            getSelectObj: function (train, dt, station_to) {
                var obj = getObjects(sending.list, 'st_lock_train', train);
                if (obj != null) {
                    var obj = getObjects(obj, 'dt_from_stat', dt);
                    if (obj != null) {
                        var obj = getObjects(obj, 'st_lock_id_stat', station_to);
                        if (obj != null) {
                            sending.select_obj = obj[0];
                        }
                    }
                }
            },
            clearSelect: function () {
                this.table.find('tbody tr').removeClass('selected');
            },
            loadData: function (data) {
                this.list = data;
                this.obj.clear();
                for (i = 0; i < data.length; i++) {
                    // Добавим данные о станциях
                    this.obj.row.add({
                        "st_lock_id_stat": data[i].st_lock_id_stat,
                        "stat": data[i].stat,
                        "id_stat": data[i].id_stat,
                        "st_lock_train": data[i].st_lock_train,
                        "dt_from_stat": data[i].dt_from_stat,
                        "vag_amount": data[i].vag_amount,
                    });
                };
                this.obj.draw();
            },
            enableTable: function (length) {
                if (length > 0) {
                    this.obj_table.show();
                } else {
                    this.obj_table.hide();
                }
            },
            viewTable: function (station_id, data_refresh, callback) {

                if (this.list == null | this.station_id != station_id | data_refresh == true) {
                    // Обновим данные
                    this.station_id = station_id;
                    getAsyncSendingStation(
                        station_id,
                        function (result) {
                            sending.loadData(result);
                            sending.enableTable(result.length);
                            //cars.viewTable(sending, false);
                            if (typeof callback === 'function') {
                                callback(result.length);
                            }
                        }
                        );
                } else {
                    this.enableTable(this.list.length);
                    cars.viewTable(sending, false);
                    if (typeof callback === 'function') {
                        callback(this.list.length);
                    }
                };
            },
            clearListSelect: function () {
                this.list = null;
                this.train = null;
                this.dt = null;
                this.station_to = null;
                this.select_obj = null;
            }
        },

        cars = {
            table: null,
            obj_table: null,
            obj: null,
            list: null,
            group: null,                        // Активная группа списка
            group_select: null,                  // id выбранного элемента группы
            //ways_id: null,
            //ways_list: null,
            initTable: function () {
                this.table = $('#table-list-cars');
                this.obj = this.table.DataTable({
                    "paging": false,
                    "ordering": true,
                    "info": false,
                    "select": false,
                    "filter": true,
                    "scrollY": "600px",
                    "scrollX": true,
                    //buttons: ['copy', 'excel', 'pdf'],
                    language: {
                        decimal: lang == 'en' ? "." : ",",
                        search: lang == 'en' ? "Search" : "Найти вагон:",
                    },
                    jQueryUI: true,
                    "createdRow": function (row, data, index) {
                        $(row).attr('id', data.id_oper);
                        if (data.id_oper == this.select) {
                            //$(row).addClass('selected');
                        }
                    },
                    columns: [
                        { data: "num_vag_on_way", title: "№", orderable: false, searchable: false },
                        { data: "num", title: "№ вагона", orderable: false, searchable: true },
                        { data: "rod", title: "Род вагона", orderable: false, searchable: false },                                                  //RailCars
                        { data: "owner_", title: "Собственник", width: "150px", orderable: false, searchable: false },                                              //RailCars
                        { data: "country", title: "Страна", orderable: false, searchable: false },                                                  //RailCars
                        { data: "wagon_country", title: "Страна (МТ)", orderable: false, searchable: false },                                       //Railway
                        { data: "cond", title: "Годность по прибытию", orderable: false, searchable: false },                                        //RailCars Годность по прибытию
                        { data: "gruz", title: "Род груза", orderable: false, searchable: false },                                                   //RailCars Годность по прибытию
                        { data: "CargoName", title: "Груз (МТ)", width: "300px", orderable: false, searchable: false },                              //Railway груз МТ
                        { data: "shop", title: "Цех-получатель груза", orderable: false, searchable: false },                                       //RailCars цех получатель
                        { data: "cond2", title: "Состояние", width: "150px", orderable: false, searchable: false },                                                 //RailCars состояние
                        { data: "dt_uz", title: "Дата и время готовности отправки с УЗ", width: "150px", orderable: false, searchable: false },    //Railway готовность отправки с УЗ
                        { data: "dt_amkr", title: "Дата и время принятия вагона на АМКР", width: "150px", orderable: false, searchable: false },   //RailCars принят на амкр
                        { data: "st_otpr", title: "Станция отправитель груза", width: "200px", orderable: false, searchable: false },                               //RailCars станция отправления
                        { data: "gruz_amkr", title: "Груз по прибытию на АМКР", orderable: false, searchable: false },                               //RailCars Груз по прибытию на АМКР
                        { data: "weight_gruz", title: "Вес груза", orderable: false, searchable: false },                                            //RailCars Вес груза
                        // - письма
                        { data: "date_mail", title: "Дата письма", orderable: false, searchable: false },                                            //RailCars Письма
                        { data: "n_mail", title: "№ письма", orderable: false, searchable: false },                                                  //RailCars Письма
                        { data: "text", title: "Текст письма", orderable: false, searchable: false },                                                //RailCars Письма
                        { data: "nm_stan", title: "Станция указанная в письме", orderable: false, searchable: false },                               //RailCars Письма
                        { data: "nm_sobstv", title: "Собственник указанный в письме", orderable: false, searchable: false },                         //RailCars Письма
                        // - Отправка грузов с тупиков
                        { data: "gdstait", title: "Станция назначения", orderable: false, searchable: false },                                       //RailCars Отправка грузов
                        { data: "note", title: "Примечание", orderable: false, searchable: false },                                                  //RailCars Отправка грузов
                        { data: "nazn_country", title: "Страна назначения", orderable: false, searchable: false },                                   //RailCars Отправка грузов
                        { data: "tupik", title: "№ тупика", orderable: false, searchable: false },                                                   //RailCars Отправка грузов
                        { data: "grvu_SAP", title: "Грузоподъемность", orderable: false, searchable: false },                                        //RailCars Отправка грузов
                        { data: "ngru_SAP", title: "Грузополучатель", orderable: false, searchable: false },                                         //RailCars Отправка грузов

                        { data: "dt_on_way", title: "Дата и время постановки на путь", width: "150px", orderable: false, searchable: false },  //RailCars 
                        // - САП
                        { data: "NumNakl", title: "№ накладной (САП)", orderable: false, searchable: false },  //SAP 
                        { data: "WeightDoc", title: "Вес по документам (САП)", orderable: false, searchable: false },  //SAP 
                        { data: "DocNumReweighing", title: "Номер отвесной (САП)", orderable: false, searchable: false },  //SAP 
                        { data: "DocDataReweighing", title: "Дата отвесной (САП)", width: "150px", orderable: false, searchable: false },  //SAP 
                        { data: "WeightReweighing", title: "Вес после перевески (САП)", orderable: false, searchable: false },  //SAP 
                        { data: "DateTimeReweighing", title: "Дата и время перевески (САП)", width: "150px", orderable: false, searchable: false },  //SAP 
                        { data: "CodeMaterial", title: "Код материала (САП)", orderable: false, searchable: false },  //SAP 
                        { data: "NameMaterial", title: "Материал (САП)", width: "300px", orderable: false, searchable: false },  //SAP 
                        { data: "CodeStationShipment", title: "Код станции отправления груза (САП)", orderable: false, searchable: false },  //SAP 
                        { data: "NameStationShipment", title: "Станция отправления груза (САП)", orderable: false, searchable: false },  //SAP 
                        { data: "CodeShop", title: "Код цеха получателя груза (САП)", orderable: false, searchable: false },  //SAP 
                        { data: "NameShop", title: "Цех получатель груза (САП)", orderable: false, searchable: false },  //SAP 
                        { data: "CodeNewShop", title: "Код цеха переадр. груза (САП)", orderable: false, searchable: false },  //SAP 
                        { data: "NameNewShop", title: "Цех переадресац. груза (САП)", orderable: false, searchable: false },  //SAP 
                        { data: "PermissionUnload", title: "Разрешение на выгрузку (САП)", orderable: false, searchable: false },  //SAP 
                    ],
                });
                this.obj_table = $('DIV#table-list-cars_wrapper');

                panel.initPanel(this.obj_table);
                this.obj_table.hide();
            },
            initEventSelect: function () {
                // Определим события выбора вагонов из таблицы
                cars.table.find('tbody tr')
                    .mousedown(function () {
                        if (panel.mode.active >= 1 & panel.mode.active <= 4) {
                            $(this).toggleClass('selected');
                            cars.table.find('tbody tr').on('mouseenter', function () {
                                $(this).toggleClass('selected');
                            });
                        }
                    });
                cars.table.mouseup(function () {
                    cars.table.find('tbody tr').off('mouseenter');
                });
            },
            clearSelect: function () {
                this.table.find('tbody tr').removeClass('selected');
            },
            allSelect: function () {
                this.table.find('tbody tr').addClass('selected');
            },
            sortPosition: function () {
                if (group_list.active == 0) {
                    if (side.select == side.out_default) {
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
                    this.obj.row.add({
                        "id_oper": data[i].id_oper,
                        "num_vag_on_way": data[i].num_vag_on_way,
                        "num": data[i].num,
                        "rod": data[i].rod,
                        "owner_": data[i].owner_,
                        "country": data[i].country,
                        "wagon_country": data[i].wagon_country,
                        "cond": data[i].cond,
                        "gruz": data[i].gruz,
                        "CargoName": data[i].CargoName,
                        "shop": data[i].shop,
                        "cond2": data[i].cond2,
                        "dt_uz": data[i].dt_uz,
                        "dt_amkr": data[i].dt_amkr,
                        "st_otpr": data[i].st_otpr,
                        "gruz_amkr": data[i].gruz_amkr,
                        "weight_gruz": data[i].weight_gruz,

                        "date_mail": data[i].date_mail,
                        "n_mail": data[i].n_mail,
                        "text": data[i].text,
                        "nm_stan": data[i].nm_stan,
                        "nm_sobstv": data[i].nm_sobstv,

                        "gdstait": data[i].gdstait,
                        "note": data[i].note,
                        "nazn_country": data[i].nazn_country,
                        "tupik": data[i].tupik,
                        "grvu_SAP": data[i].grvu_SAP,
                        "ngru_SAP": data[i].ngru_SAP,

                        "dt_on_way": data[i].dt_on_way,

                        "NumNakl": data[i].NumNakl,
                        "WeightDoc": data[i].WeightDoc,
                        "DocNumReweighing": data[i].DocNumReweighing,
                        "DocDataReweighing": data[i].DocDataReweighing,
                        "WeightReweighing": data[i].WeightReweighing,
                        "DateTimeReweighing": data[i].DateTimeReweighing,
                        "CodeMaterial": data[i].CodeMaterial,
                        "NameMaterial": data[i].NameMaterial,
                        "CodeStationShipment": data[i].CodeStationShipment,
                        "NameStationShipment": data[i].NameStationShipment,
                        "CodeShop": data[i].CodeShop,
                        "NameShop": data[i].NameShop,
                        "CodeNewShop": data[i].CodeNewShop,
                        "NameNewShop": data[i].NameNewShop,
                        "PermissionUnload": data[i].PermissionUnload,
                    });
                }
                this.sortPosition();
                this.initEventSelect();
                //this.obj.draw();
            },
            enableColumsMT: function (view) {
                this.obj.columns([5, 8]).visible(view, true);
                this.obj.draw(false);
            },
            enableColumsSAP: function (view) {
                this.obj.columns([28,29,30,31,32,33,34,35,36,37,38,39,40,41,42]).visible(view, true);
                this.obj.draw(false);
            },
            enableColumsEMAIL: function (view) {
                this.obj.columns([16, 17, 18, 19, 20]).visible(view, true);
                this.obj.draw(false);
            },
            enableColumsOutCars: function (view) {
                this.obj.columns([21, 22, 23, 24, 25, 26]).visible(view, true);
                this.obj.draw(false);
            },
            enableTable: function (length) {
                if (length >= 0) {
                    this.obj_table.show();
                } else {
                    this.obj_table.hide();
                }
            },
            viewTable: function (obj_select, data_refresh) {
                if (typeof obj_select == 'object') {
                    // Показать выбранный путь
                    if (obj_select.name == 'list_ways' & group_list.active == 0) {
                        if (cars.list == null | data_refresh == true | cars.group != 'list_ways' | (cars.group == 'list_ways' & cars.group_select != obj_select.select)) {
                            cars.group_select = obj_select.select;
                            cars.group = 'list_ways';
                            // Загружаем
                            getAsyncCarsOfWay(
                                (obj_select.select != null ? obj_select.select : 0),
                                (side.select == side.out_default ? 1 : 0),
                                function (result) {
                                    panel.info.viewInfo(obj_select);
                                    cars.loadData(result);
                                    cars.enableTable(result.length);
                                });
                        } else {
                            //cars.loadData(cars.list);
                            cars.enableTable(cars.list.length);
                        }
                    }
                    // Показать выбранный вагоноопрокид
                    if (obj_select.name == 'list_wagonoverturns' & group_list.active == 1) {
                        if (cars.list == null | data_refresh == true | cars.group != 'list_wagonoverturns' | (cars.group == 'list_wagonoverturns' & cars.group_select != obj_select.select)) {
                            cars.group_select = obj_select.select;
                            cars.group = 'list_wagonoverturns';
                            // Загружаем
                            getAsyncCarsOfWagonOverturn(
                                (obj_select.select != null ? obj_select.select : 0),
                                function (result) {
                                    panel.info.viewInfo(obj_select);
                                    // Коррекция нумерации вагонов
                                    for (i = 0; i < result.length; i++) {
                                        result[i].num_vag_on_way = i + 1;
                                    }
                                    cars.loadData(result);
                                    cars.enableTable(result.length);
                                });
                        } else {
                            //cars.loadData(cars.list);
                            cars.enableTable(cars.list.length);
                        }

                    }
                    // Показать выбранный цех-тупик
                    if (obj_select.name == 'list_shops' & group_list.active == 2) {
                        if (cars.list == null | data_refresh == true | cars.group != 'list_shops' | (cars.group == 'list_shops' & cars.group_select != obj_select.select)) {
                            cars.group_select = obj_select.select;
                            cars.group = 'list_shops';
                            // Загружаем
                            getAsyncCarsOfShop(
                                (obj_select.select != null ? obj_select.select : 0),
                                function (result) {
                                    panel.info.viewInfo(obj_select);
                                    cars.loadData(result);
                                    cars.enableTable(result.length);
                                });
                        } else {
                            //cars.loadData(cars.list);
                            cars.enableTable(cars.list.length);
                        }
                    }
                    // Показать прибытие
                    if (obj_select.name == 'list_arrival_amkr' & group_list.active == 3) {
                        if (cars.list == null | data_refresh == true | cars.group != 'list_arrival_amkr' | (cars.group == 'list_arrival_amkr' & cars.group_select != obj_select.train)) {
                            cars.group_select = obj_select.train;
                            cars.group = 'list_arrival_amkr';
                            // Загружаем
                            getAsyncCarsOfArrivalAMKR(
                                (obj_select.station_id != null ? obj_select.station_id : 0),
                                (obj_select.train != null ? obj_select.train : 0),
                                (obj_select.dt != null ? obj_select.dt : new Date()),
                                0,
                                function (result) {
                                    panel.info.viewInfo(obj_select);
                                    // Коррекция нумерации вагонов
                                    for (i = 0; i < result.length; i++) {
                                        result[i].num_vag_on_way = i + 1;
                                    }
                                    cars.loadData(result);
                                    cars.enableTable(result.length);
                                });
                        } else {
                            //cars.loadData(cars.list);
                            cars.enableTable(cars.list.length);
                        }
                    }
                    // Показать прибытие УЗ
                    if (obj_select.name == 'list_arrival_uz' & group_list.active == 4) {
                        if (cars.list == null | data_refresh == true | cars.group != 'list_arrival_uz' | (cars.group == 'list_arrival_uz' & cars.group_select != obj_select.select)) {
                            cars.group_select = obj_select.select;
                            cars.group = 'list_arrival_uz';
                            // Загружаем
                            getAsyncCarsOfArrivalUZ(
                                (obj_select.select != null ? obj_select.select : 0),
                                true,
                                function (result) {
                                    panel.info.viewInfo(obj_select);
                                    // Коррекция нумерации вагонов
                                    for (i = 0; i < result.length; i++) {
                                        result[i].num_vag_on_way = i + 1;
                                    }
                                    cars.loadData(result);
                                    cars.enableTable(result.length);
                                });
                        } else {
                            //cars.loadData(cars.list);
                            cars.enableTable(cars.list.length);
                        }
                    }
                    // Показать отправление
                    if (obj_select.name == 'list_sending' & group_list.active == 5) {
                        if (cars.list == null | data_refresh == true | cars.group != 'list_sending' | (cars.group == 'list_sending' & cars.group_select != obj_select.train + obj_select.dt)) {
                            cars.group_select = obj_select.train + obj_select.dt;
                            cars.group = 'list_sending';
                            // Загружаем
                            getAsyncCarsOfSending(
                                (obj_select.station_id != null ? obj_select.station_id : 0),
                                (obj_select.train != null ? obj_select.train : 0),
                                (obj_select.dt != null ? obj_select.dt : new Date()),
                                (obj_select.station_to != null ? obj_select.station_to : 0),
                                function (result) {
                                    panel.info.viewInfo(obj_select);
                                    //// Коррекция нумерации вагонов
                                    for (i = 0; i < result.length; i++) {
                                        result[i].num_vag_on_way = i + 1;
                                    }
                                    cars.loadData(result);
                                    cars.enableTable(result.length);
                                });
                        } else {
                            //cars.loadData(cars.list);
                            cars.enableTable(cars.list.length);
                        }
                    }
                }
            },
        }

    //-----------------------------------------------------------------------------------------
    // Функции
    //-----------------------------------------------------------------------------------------
    // Функция уберает выделенные строки в таблицах списках групп
    function clearGroupSelectTable() {
        ways.clearSelect();
        wagonoverturns.clearSelect();
        shops.clearSelect();
    }
    //  Показать группу списков
    function viewGroup(active_group, station_id, data_refresh) {
        panel.activePanel(active_group);
        switch (active_group) {
            case 0:
                viewGroupWays(station_id, data_refresh);
                break;
            case 1:
                viewGroupWagonOverturns(station_id, data_refresh);
                break;
            case 2:
                viewGroupShops(station_id, data_refresh);
                break;
            case 3:
                viewGroupArrivalAMKR(station_id, data_refresh);
                break;
            case 4:
                viewGroupArrivalUZ(station_id, data_refresh);
                break;
            case 5:
                viewGroupSending(station_id, data_refresh);
                break;

            default:
                // Группы закрыты
                cars.enableTable(-1);
                panel_detali.removeClass(); // Убрать подкраску режимов
                break;
        }
    }
    // Показать группу путей
    function viewGroupWays(station_id, data_refresh) {
        group_list.list_ways.show();
        ways.viewTable(station_id, data_refresh);
    }
    // Показать группу вагоноопрокидов
    function viewGroupWagonOverturns(station_id, data_refresh) {
        wagonoverturns.viewTable(
            station_id,
            data_refresh,
            function (result) {
                if (result > 0) {
                    group_list.list_wagonoverturns.show();
                } else {
                    group_list.list_wagonoverturns.hide();
                }
            }
            );
    }
    // Показать группу цехов-тупиков
    function viewGroupShops(station_id, data_refresh) {
        shops.viewTable(
            station_id,
            data_refresh,
            function (result) {
                if (result > 0) {
                    group_list.list_shops.show();
                } else {
                    group_list.list_shops.hide();
                }
            }
            );
    }
    // Показать группу прибытие с АМКР
    function viewGroupArrivalAMKR(station_id, data_refresh) {
        arrival_amkr.viewTable(
            station_id,
            data_refresh,
            null
            //function (result) {
            //    if (result > 0) {
            //        group_list.list_arrival.show();
            //    } else {
            //        group_list.list_arrival.hide();
            //    }
            //}
            );
    }

    function viewGroupArrivalUZ(station_id, data_refresh) {
        arrival_uz.viewTable(
            station_id,
            data_refresh,
            null
            //function (result) {
            //    if (result > 0) {
            //        group_list.list_arrival.show();
            //    } else {
            //        group_list.list_arrival.hide();
            //    }
            //}
            );
    }

    // Показать группу отправки вагонов
    function viewGroupSending(station_id, data_refresh) {
        sending.viewTable(
            station_id,
            data_refresh,
            null
            //function (result) {
            //    if (result > 0) {
            //        group_list.list_sending.show();
            //    } else {
            //        group_list.list_sending.hide();
            //    }
            //}
            );
    }

    //-----------------------------------------------------------------------------------------
    // Инициализация объектов
    //-----------------------------------------------------------------------------------------

    station.initStation(true);
    nodes.initNodes();

    group_list.initGroup();             // Инициализируем групы списков
    ways.initTable();                   // Инициализируем таблицу путей
    wagonoverturns.initTable();         // Инициализируем таблицу вагоноопрокидов
    shops.initTable();                  // Инициализируем таблицу цеха тупики
    arrival_amkr.initTable();           // Инициализируем таблицу прибытие амкр
    arrival_uz.initTable();             // Инициализируем таблицу прибытие уз
    sending.initTable();                // Инициализируем таблицу отправки амкр

    cars.initTable();                   // Инициализируем таблицу вагоны

    //Загрузим горловину
    getAsyncSide(function (result) {
        side.list = result;
        initSelect(
            side.obj,
            { width: 150 },
            result,
            null,
            side.select,
            function (event, ui) {
                event.preventDefault();
                side.select = Number(ui.item.value);
                cars.sortPosition();
                // Показать вагоны с учетом изменения стороны (сделал сортировкой для увеличения скорости)
                //if (side.select == side.out_default) {
                //    obj_table_list_cars.order([0, 'desc']);
                //} else {
                //    obj_table_list_cars.order([0, 'asc']);
                //}
                //obj_table_list_cars.draw();
            },
            null);
    });
});

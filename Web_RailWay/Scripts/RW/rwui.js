//$(document).ready(function () {
$(function () {

    //-----------------------------------------------------------------------------------------
    // Объявление глобальных переменных
    //-----------------------------------------------------------------------------------------
    //allVars = $.getUrlVars(),   // Получить параметры get запроса
    var lang = $.cookie('lang'),
        tabs, group_list,
        select_stations = $('select[name ="station"]'), // компонент select station
        list_station,                                   // Список станций загружается при заполнении компонента select station
        id_station = -1,                                // id станции
        id_station_rc = -1,                             // id станции Railcars
        name_station = null,
        exit_uz = false,                              // Признак выхода на УЗ
        select_side = $('select[name ="side"]'),        // компонент select side
        side_station = 0,                               // Сторона
        default_side = 0,                               // Сторона указаная при загрузке станции
        label_station_name = $('#station-name'),
        left_panel = $('div#left-operation'),
        right_panel = $('div#right-operation'),
        operation_panel = $('div#content-operation'),
        panel_detali = $('div#operation-detali'),
        panel_setup_operation,                          // Панель для элементов настройки операций

        tabs_active = 0,
        group_active = 0,

        list_ways_station,                  // список путей станции
        list_shops_station,                 // список цехов станции
        list_wagonoverturn_station,         // список вагоноопрокидов станции
        list_cars,                          // список вагонов на пути
        list_gorlov,                        // список горловин

        table_list_ways = $('#table-list-ways'),
        div_table_list_ways,
        obj_table_list_ways,                                            // Таблица путей станции

        table_list_wagonoverturns = $('#table-list-wagonoverturns'),
        div_table_list_wagonoverturns,
        obj_table_list_wagonoverturns,                                  // Таблица вагоноопрокидов станции

        table_list_shops = $('#table-list-shops'),
        div_table_list_shops,
        obj_table_list_shops,

        // Таблица цехов станции
        table_list_cars = $('#table-list-cars'),            // Таблица вагонов
        obj_table_list_cars,
        way_select_id = null,                               // Выбранный путь
        wagonoverturns_select_id = null,                    // Выбранный вагоноопрокид
        shops_select_id  = null,                            // Выбран цех
        group_list_ways = $('#group-list-ways'),                        // Группа элементов отображения информации о путях
        group_list_wagonoverturns = $('#group-list-wagonoverturns'),    // Группа элементов отображения информации о вагоноопрокидах
        group_list_shops = $('#group-list-shops'),                      // Группа элементов отображения информации о цехах
        // панель настроек маневра
        button_select_all = $('<button class="ui-button ui-widget ui-corner-all">' + (lang == 'en' ? "Select All" : "Выбрать все вагоны") + '</button>'),
        button_clear_all = $('<button class="ui-button ui-widget ui-corner-all">' + (lang == 'en' ? "Clear All" : "Убрать все вагоны") + '</button>'),
        label_gorlov_manevr = $('<label class="setup-label">' + (lang == 'en' ? "The throat of maneuver:" : "Горловина маневра:") + '</label>'),
        label_way_manevr = $('<label class="setup-label">' + (lang == 'en' ? "Maneuver on the way:" : "Маневр на путь:") + '</label>'),
        select_manevr_side = $('<select id="manevr-side" name="station"></select>'),
        select_manevr_way = $('<select id="manevr-way" name="station"></select>'),
        button_ok_manevr = $('<button class="ui-button ui-widget ui-corner-all">' + (lang == 'en' ? "Perform a maneuver" : "Выполнить маневр") + '</button>')
    panel_property_manevr_operation = $('<div class="dt-buttons setup-operation" id="property_manevr_operation" style="width:100%"></div>')
        .append(button_select_all)
        .append(button_clear_all)
        .append(label_gorlov_manevr)
        .append(select_manevr_side)
        .append(label_way_manevr)
        .append(select_manevr_way)
        .append(button_ok_manevr),
    // панель настроек отображения информации
        label_view_select = $('<label id="view-info">Информация:</label>'),
        label_view_mt = $('<label for="view-mt">' + (lang == 'en' ? "MetallurgTrans " : "МеталургТранс ") + '</label>'),
        checkbox_view_mt = $('<input type="checkbox" name="view-mt" id="view-mt">'),
        label_view_sap = $('<label for="view-sap">' + (lang == 'en' ? "SAP " : "САП ") + '</label>'),
        checkbox_view_sap = $('<input type="checkbox" name="view-sap" id="view-sap">'),
        label_view_email = $('<label for="view-email">' + (lang == 'en' ? "Writing  " : "Письма ") + '</label>')
        checkbox_view_email = $('<input type="checkbox" name="view-email" id="view-email">'),
        panel_property_view_cars = $('<div class="dt-buttons setup-operation" id="property_view_cars"></div>')
        .append(label_view_select)
        .append(label_view_mt).append(checkbox_view_mt)
        .append(label_view_sap).append(checkbox_view_sap)
        .append(label_view_email).append(checkbox_view_email)

    // Получим id станции
    //if (allVars != null) {
    //    id_station = Number(allVars.id);
    //}

    //id_station = 6; //!!!!!!! убрать
    //-----------------------------------------------------------------------------------------
    // Функции
    //-----------------------------------------------------------------------------------------
    //Получить свойства станции по id из списка станций и отобразить информацию  
    function setStationProperty(id, obj) {
        var station = getObjects(obj, 'id', id)
        if (station != null && station.length > 0) {
            id_station = id;
            id_station_rc = station[0].id_rs;
            name_station = lang == 'en' ? station[0].name_en : station[0].name_ru; //label_station_name.text(name_station);
            exit_uz = station[0].exit_uz;
            // определим горловину по умолчанию
            default_side = 1; // Если в таблице null будет определенно как 1
            side_station = 0
            if (station[0].default_side == true) {
                default_side = 1;
                side_station = 0
            }
            if (station[0].default_side == false) {
                default_side = 0;
                side_station = 1
            }
            select_side.val(side_station).selectmenu("refresh");
            // !! добавить определение выхода на УЗ
        }
    }
    // Информация о выбранном пути\вагоноопрокиде\цехе
    function getInfoSelect(way_select_id, wagonoverturns_select_id, shops_select_id) {
        if (way_select_id != null) {
            var way = getObjects(list_ways_station, 'id_way', way_select_id)
            return 'Выбран путь : '+ way[0].num+ ' - '+way[0].name;
        }
        if (wagonoverturns_select_id != null) {
            var way = getObjects(list_wagonoverturn_station, 'id_gruz_front', wagonoverturns_select_id)
            return 'Выбран вагоноопрокид : '+way[0].name;
        }
        if (shops_select_id != null) {
            var way = getObjects(list_shops_station, 'id_shop', shops_select_id)
            return 'Выбран цех : '+way[0].name;
        }
    }

    function clearSelectedGroup() {
        way_select_id = null;               // Очистим  Выбранный путь
        wagonoverturns_select_id = null;    // Очистим  Выбранный вагоноопрокид
        shops_select_id = null;             // Очистим  Выбран цех
        table_list_ways.find('tbody tr').removeClass('selected');
        table_list_wagonoverturns.find('tbody tr').removeClass('selected');
        table_list_shops.find('tbody tr').removeClass('selected');
    }

    // Обновить данными таблицу путей
    function viewTableWaysStations(data) {

        obj_table_list_ways.clear();
        for (i = 0; i < data.length; i++) {
            obj_table_list_ways.row.add({
                "id_way": data[i].id_way,
                "num": data[i].num,
                "name": data[i].name,
                "vag_amount": data[i].vag_amount,
                "vag_capacity": data[i].vag_capacity,
            });
        }
        obj_table_list_ways.draw();
        visibleTableWaysStations()
    };

    // Показать\спрятать таблицу путей 
    function visibleTableWaysStations() {
        if (list_ways_station != null && list_ways_station.length > 0 & (tabs_active == 0 | tabs_active == 1 | tabs_active == 2)) {
            group_list_ways.show();
            div_table_list_ways.show()
            //if (tabs_active == 0) {
            //    $("#group-list").accordion({ active: 0 });
            //}
        } else {
            group_list_ways.hide();
            div_table_list_ways.hide();
        }
    }

    // Обновить данными таблицу вагоноопрокидов
    function viewTableWagonOverturnsStations(data) {

        obj_table_list_wagonoverturns.clear();
        for (i = 0; i < data.length; i++) {
            obj_table_list_wagonoverturns.row.add({
                "id_gruz_front": data[i].id_gruz_front,
                "name": data[i].name,
                "vag_amount": data[i].vag_amount,
            });
        }
        obj_table_list_wagonoverturns.draw();
        visibleTableWagonOverturnsStations();
    };

    // Показать\спрятать таблицу вагоноопрокидов
    function visibleTableWagonOverturnsStations() {
        if (list_wagonoverturn_station != null && list_wagonoverturn_station.length > 0 & tabs_active == 0) {
            group_list_wagonoverturns.show();
            div_table_list_wagonoverturns.show();

        } else {
            group_list_wagonoverturns.hide();
            div_table_list_wagonoverturns.hide();
        }
    }

    // Обновить данными таблицу цехов
    function viewTableShopsStations(data) {
        obj_table_list_shops.clear();
        for (i = 0; i < data.length; i++) {
            obj_table_list_shops.row.add({
                "id_shop": data[i].id_shop,
                "name": data[i].name,
                "vag_amount": data[i].vag_amount,
            });
        }
        obj_table_list_shops.draw();
        visibleTableShopsStations();
    };

    // Показать\спрятать таблицу цехов
    function visibleTableShopsStations() {
        if (list_shops_station != null && list_shops_station.length > 0 & tabs_active == 0) {
            group_list_shops.show();
            div_table_list_shops.show();
        } else {
            group_list_shops.hide();
            div_table_list_shops.hide();
        }
    }
    // Показать таблицу вагонов на путях
    function viewTableCarsOfway(data) {
        OnBegin();
        operation_panel.show();
        obj_table_list_cars.clear();
        for (i = 0; i < data.length; i++) {
            obj_table_list_cars.row.add({
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

        if (way_select_id != null) {
            // Если выбран путь
            if (side_station == default_side) {
                obj_table_list_cars.order([0, 'desc']);
            } else {
                obj_table_list_cars.order([0, 'asc']);
            }
        } else {
            // Если выбран цех или вагоноопрокид
            obj_table_list_cars.order([0, 'asc']);
        }

        obj_table_list_cars.draw();

        eventSelectCars(tabs_active)

        LockScreenOff();
    };

    // выбрать вагоны по указанному пути с учетом горловины
    function viewCars(way_select_id, wagonoverturns_select_id, shops_select_id, side_station, default_side) {
        label_view_select.text(getInfoSelect(way_select_id, wagonoverturns_select_id, shops_select_id));
        if (way_select_id != null) {
            getAsyncCarsOfWay(way_select_id, (side_station == default_side ? 1 : 0),
                function (result) {
                    list_cars = result;
                    viewTableCarsOfway(list_cars);
                });
        };
        if (wagonoverturns_select_id != null) {
            getAsyncCarsOfWagonOverturn(wagonoverturns_select_id,
                function (result) {
                    // Коррекция нумерации вагонов
                    for (i = 0; i < result.length; i++) {
                        result[i].num_vag_on_way = i + 1;
                    }
                    list_cars = result;
                    viewTableCarsOfway(list_cars);
                });
        };
        if (shops_select_id != null) {
            getAsyncCarsOfShop(shops_select_id,
                function (result) {
                    list_cars = result;
                    viewTableCarsOfway(list_cars);
                });
        };
    };
    // Отабазим дополнительные элементы настройки операций 
    function viewObjSetupOperation(tabs_active) {
        switch (tabs_active) {
            case 1:
                panel_property_manevr_operation.show();
                button_select_all.on('click', function () {
                    $('#table-list-cars tbody tr').addClass('selected');
                });
                button_clear_all.on('click', function () {
                    $('#table-list-cars tbody tr').removeClass('selected');
                });
                initSelect(select_manevr_side, { width: 150 }, list_gorlov, null, 0, function (event, ui) { event.preventDefault(); }, null);
                initSelect(select_manevr_way, { width: 300 }, list_ways_station, function (row) { return { value: row.id_way, text: row.num + '-' + row.name }; }, -1, function (event, ui) { event.preventDefault(); }, null);
                break;
            case 2:
                break;
            default:
                break;
        }
    }
    // Определить события выбора вагонов в зависимости от выбранной операции
    function eventSelectCars(tabs_active) {
        // Определим события выбора вагонов из таблицы
        if (tabs_active >= 1 & tabs_active <= 2) {
            table_list_cars.find('tbody tr')
            .mousedown(function () {
                $(this).toggleClass('selected');
                table_list_cars.find('tbody tr').on('mouseenter', function () {
                    $(this).toggleClass('selected');
                });
            });
            table_list_cars.mouseup(function () {
                table_list_cars.find('tbody tr').off('mouseenter');
            });
            //table_list_cars.find('tbody').mouseout(function () {
            //    table_list_cars.find('tbody tr').off('mouseenter');
            //});
        } else {
            table_list_cars.find('tbody tr').off('mousedown').off('mouseup').removeClass('selected');
        }
    };

    // Показать выбранную операцию
    function viewOperation(tabs_active) {
        // Операции
        if (tabs_active >= 0 & tabs_active <= 2) {
            right_panel.hide();
            // определим размер панели
            operation_panel.width(panel_detali.width() - left_panel.outerWidth() - 20);
            // если данных по путям -нет, получить
            if (list_ways_station == null) {
                getAsyncWaysStation(
                    id_station_rc,
                    false,
                    function (result) {
                        list_ways_station = result;
                        // Если есть данные и панел == 0 тогда паказываем таблицу
                        //if (list_ways_station.length > 0 & tabs_active == 0) { group_list_ways.show(); } else { group_list_ways.hide(); }
                        //visibleTableWaysStations();
                        viewTableWaysStations(list_ways_station);
                    }
                    );
            } else {
                visibleTableWaysStations();
            }

            // если данных по цехам -нет, получить
            if (list_shops_station == null) {
                getAsyncShopStation(
                    id_station_rc,
                    function (result) {
                        list_shops_station = result;
                        //if (list_shops_station.length > 0 & tabs_active == 0) { group_list_shops.show(); } else { group_list_shops.hide(); }
                        //visibleTableShopsStations();
                        viewTableShopsStations(list_shops_station);
                    }
                    );
            } else {
                visibleTableShopsStations();
            }
            
            // если данных по вагоноопрокидам -нет, получить
            if (list_wagonoverturn_station == null) {
                getAsyncWagonOverturnsStation(
                    id_station_rc,
                    function (result) {
                        list_wagonoverturn_station = result;
                        //if (list_wagonoverturn_station.length > 0 & tabs_active == 0) { group_list_wagonoverturns.show(); } else { group_list_wagonoverturns.hide(); }
                        //visibleTableWagonOverturnsStations();
                        viewTableWagonOverturnsStations(list_wagonoverturn_station);
                    }
                    );
            } else {
                visibleTableWagonOverturnsStations();
            }
            
            // Показывать\скрывать столбец вместимость
            if (tabs_active == 0) {
                obj_table_list_ways.column(3).visible(false);
            } else {
                obj_table_list_ways.column(3).visible(true);
            }
            // Перерисовать
            obj_table_list_ways.draw();
            // Определить события выбора вагонов в зависимости от выбранной операции
            eventSelectCars(tabs_active)
            //Определим дополнительные элементы настройки операции 
            viewObjSetupOperation(tabs_active);
        }
    }

    //-----------------------------------------------------------------------------------------
    // Подготовка окна
    //-----------------------------------------------------------------------------------------

    // Настроим Табс ---------------------------------------------------------------------------------------------
    tabs = $("#tabs-operation").tabs({
        activate: function (event, ui) {
            tabs_active = tabs.tabs("option", "active");
            viewOperation(tabs_active);
        }
    });

    // Показать группированный список (путей\вагоноопрокидов\цехов) -----------------------------------------------
    group_list = $("#group-list").accordion({
        heightStyle: "content",
        activate: function (event, ui) {
            group_active = group_list.accordion("option", "active");
        }

    });
    // Скрыть все группы
    group_list_ways.hide();
    group_list_wagonoverturns.hide();
    group_list_shops.hide();

    // Инициализация таблицы пути ---------------------------------------------------------------------------------------------
    obj_table_list_ways = table_list_ways.DataTable({
        "paging": false,
        "ordering": false,
        "info": false,
        "select": false,
        "filter": false,
        language: {
            //decimal: lang == 'en' ? "." : ",",
            //search: "Search" : "Найти",
            emptyTable: lang == 'en' ? "No data available in table" : "Данные отсутствуют",
        },
        jQueryUI: true,
        //data: data,
        "createdRow": function (row, data, index) {
            $(row).attr('id', data.id_way);
            if (data.id_way == way_select_id) {
                $(row).addClass('selected');
                viewCars(way_select_id, wagonoverturns_select_id, shops_select_id, side_station, default_side);
            }
        },
        columns: [
            { data: "num", title: "№", width: "30px" },
            { data: "name", title: "Путь" },
            { data: "vag_amount", title: "Кол. ваг.", width: "30px" },
            { data: "vag_capacity", title: "Вмест. ваг.", width: "30px" },
        ],
    });
    // инициализировали переменную div_table
    div_table_list_ways = $('DIV#table-list-ways_wrapper');
    div_table_list_ways.hide();

    // Определим событие выбора пути станции
    table_list_ways.find('tbody')
    .on('click', 'tr', function () {
        clearSelectedGroup();
        $(this).addClass('selected');
        way_select_id = $(this).attr("id");
        viewCars(way_select_id, wagonoverturns_select_id, shops_select_id, side_station, default_side);
    });

    // Инициализация таблицы вагоноопрокиды ---------------------------------------------------------------------------------------------
    obj_table_list_wagonoverturns = table_list_wagonoverturns.DataTable({
        "paging": false,
        "ordering": false,
        "info": false,
        "select": false,
        "filter": false,
        language: {
            emptyTable: lang == 'en' ? "No data available in table" : "Данные отсутствуют",
        },
        jQueryUI: true,
        "createdRow": function (row, data, index) {
            $(row).attr('id', data.id_gruz_front);
            if (data.id_gruz_front == wagonoverturns_select_id) {
                $(row).addClass('selected');
                viewCars(way_select_id, wagonoverturns_select_id, shops_select_id, side_station, default_side);
            }
        },
        columns: [
            { data: "name", title: "Путь" },
            { data: "vag_amount", title: "Кол. ваг.", width: "30px" },
        ],
    });
    // инициализировали переменную div_table
    div_table_list_wagonoverturns = $('DIV#table-list-wagonoverturns_wrapper');
    div_table_list_wagonoverturns.hide();

    // Определим событие выбора вагоноопрокида станции
    table_list_wagonoverturns.find('tbody')
    .on('click', 'tr', function () {
        clearSelectedGroup();
        $(this).addClass('selected');
        wagonoverturns_select_id = $(this).attr("id");
        viewCars(way_select_id, wagonoverturns_select_id, shops_select_id, side_station, default_side);
    });

    // Инициализация таблицы цеха ---------------------------------------------------------------------------------------------
    obj_table_list_shops = table_list_shops.DataTable({
        "paging": false,
        "ordering": false,
        "info": false,
        "select": false,
        "filter": false,
        language: {
            emptyTable: lang == 'en' ? "No data available in table" : "Данные отсутствуют",
        },
        jQueryUI: true,
        "createdRow": function (row, data, index) {
            $(row).attr('id', data.id_shop);
            if (data.id_shop == shops_select_id) {
                $(row).addClass('selected');
                viewCars(way_select_id, wagonoverturns_select_id, shops_select_id, side_station, default_side);
            }
        },
        columns: [
            { data: "name", title: "Путь" },
            { data: "vag_amount", title: "Кол. ваг.", width: "30px" },
        ],
    });
    // инициализировали переменную div_table
    div_table_list_shops = $('DIV#table_list_shops_wrapper');
    div_table_list_shops.hide();
    
    // Определим событие выбора цеха
    table_list_shops.find('tbody')
    .on('click', 'tr', function () {
        clearSelectedGroup();
        $(this).addClass('selected');
        shops_select_id = $(this).attr("id");
        viewCars(way_select_id, wagonoverturns_select_id, shops_select_id, side_station, default_side);
    });


    // Инициализация таблицы вагоны ---------------------------------------------------------------------------------------------
    obj_table_list_cars = table_list_cars.DataTable({
        "paging": false,
        "ordering": true,
        "info": false,
        "select": false,
        "filter": true,
        "scrollY": "600px",
        "scrollX": true,
        buttons: ['copy', 'excel', 'pdf'],
        language: {
            decimal: lang == 'en' ? "." : ",",
            search: lang == 'en' ? "Search" : "Найти вагон:",
        },
        jQueryUI: true,
        "createdRow": function (row, data, index) {
            $(row).attr('id', data.id_oper);
            if (data.id_oper == way_select_id) {
                $(row).addClass('selected');
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


//public int? CountryCode {get;set;}
//public int? PostReweighing {get;set;}
//public int? CodeCargo {get;set;}
//public bool? Step1 {get;set;}
//public bool? Step2 {get;set;}
//public int? n_natur {get;set;}
//public int? id_vagon {get;set;}
//public int? id_stat {get;set;}
//public DateTime? dt_from_stat {get;set;}
//public DateTime? dt_on_stat {get;set;}
//public int? id_way {get;set;}
//public DateTime? dt_from_way {get;set;}
//public DateTime? dt_on_way {get;set;}
//public int? is_present {get;set;}
//public int? id_locom {get;set;}
//public int? id_locom2 {get;set;}
//public int? id_cond2 {get;set;}
//public int? id_gruz {get;set;}
//public int? id_gruz_amkr {get;set;}
//public int? id_shop_gruz_for {get;set;}
//public int? id_tupik {get;set;}
//public int? id_nazn_country {get;set;}
//public int? id_gdstait {get;set;}
//public int? id_cond {get;set;}
//public int? is_hist {get;set;}
//public int? id_oracle {get;set;}
//public int? lock_id_way {get;set;}
//public int? lock_order {get;set;}
//public int? lock_side {get;set;}
//public int? lock_id_locom {get;set;}
//public int? st_lock_id_stat {get;set;}
//public int? st_lock_order {get;set;}
//public int? st_lock_train {get;set;}
//public int? st_lock_side {get;set;}
//public int? st_gruz_front {get;set;}
//public int? st_shop {get;set;}
//public int? oracle_k_st {get;set;}
//public int? st_lock_locom1 {get;set;}
//public int? st_lock_locom2 {get;set;}
//public int? id_oper_parent {get;set;}
//public int? id_ora_23_temp {get;set;}
//public string edit_user {get;set;}
//public DateTime? edit_dt {get;set;}
//public int? IDSostav {get;set;}
//public int? num_vagon {get;set;}
//public DateTime? dt_out_amkr {get;set;}
//public int? id_cond_after {get;set;}
        ],
    });
    // Добавим div для дополнительных настроек параметров
    //$('div#table-list-cars_wrapper').prepend('<div class="dt-buttons" id="setup-operation"></div>');
    //panel_setup_operation = $('div#setup-operation');
    $('div#table-list-cars_wrapper').prepend(panel_property_view_cars.controlgroup());
    $('div#table-list-cars_wrapper').prepend(panel_property_manevr_operation.hide());


    operation_panel.hide();

    //Загрузим выбор станций
    getAsyncViewStations(function (result) {
        list_station = result; // Сохраним список станций
        initSelect(
            select_stations,
            { width: 300 },
            result,
            function (row) {
                return { value: row.id, text: (lang == 'en' ? row.name_en : row.name_ru) };
            },
            id_station,
            function (event, ui) {
                event.preventDefault();
                id_station = Number(ui.item.value);
                // Должны выбрать станцию
                if (id_station > 0) {
                    operation_panel.hide(); // скрыть панель с вагонами
                    setStationProperty(id_station, list_station);
                    // Очистим переменные
                    list_ways_station = null;           // Очистим список путей
                    list_shops_station = null;          // Очистим список цехов станции
                    list_wagonoverturn_station = null;  // Очистим список вагоноопрокидов станции
                    list_cars = null;                   // Очистим список вагонов
                    way_select_id = null;               // Очистим  Выбранный путь
                    wagonoverturns_select_id = null;    // Очистим  Выбранный вагоноопрокид
                    shops_select_id = null;             // Очистим  Выбран цех
                    // Показывать группу пути по умолчанию
                    $("#group-list").accordion({ active: 0 });
                    viewOperation(tabs_active);
                }
            },
            null);
    });
    //Загрузим горловину
    getAsyncSide(function (result) {
        list_gorlov = result;
        initSelect(
            select_side,
            { width: 150 },
            result,
            null,
            side_station,
            function (event, ui) {
                event.preventDefault();
                side_station = Number(ui.item.value);
                // Показать вагоны с учетом изменения стороны (сделал сортировкой для увеличения скорости)
                if (side_station == default_side) {
                    obj_table_list_cars.order([0, 'desc']);
                } else {
                    obj_table_list_cars.order([0, 'asc']);
                }
                obj_table_list_cars.draw();
            },
            null);
    });

});
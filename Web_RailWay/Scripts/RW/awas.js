$(function () {
    //-----------------------------------------------------------------------------------------
    // Объявление глобальных переменных
    //-----------------------------------------------------------------------------------------
    var lang = $.cookie('lang'),
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
        // Группы спиков
        tab_group_list = {
            html_div: $("#group-list"),
            active: 0,
            initObject: function () {
                //this.obj =
                this.html_div.accordion({
                    collapsible: true,
                    heightStyle: "content",
                    activate: function (event, ui) {
                        tab_group_list.active = tab_group_list.html_div.accordion("option", "active");
                        //viewGroup(group_list.active, station.id_rc, false)
                    },
                });
                group_list_ways.hide();
                group_list_wo.hide();
                group_list_shops.hide();
                group_list_arrival.hide();
                group_list_arrival_uz.hide();
                group_list_sending.hide();
            },
            activeOfStation: function (station) {
                if (station != null) {
                    if (station.exit_uz) {
                        group_list_arrival_uz.show();
                    } else { group_list_arrival_uz.hide() }
                }
            },
        },
        // группа список путей
        group_list_ways = {
            html_div: $("#group-list-ways"),
            hide: function () {
                this.html_div.hide();
            },
            show: function () {
                this.html_div.show();
            },
        },
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
        // class ArrivalSostav
        arrival_sostav = {
            id_sostav: null,
            id_arrival: null,
            index: null,
            dt_inp_station: null,
            cars: null,
        },
        // таблица прибытия на станци УЗ
        table_arrival_uz = {
            //name: 'list_arrival_uz',
            station_id: null,
            select: arrival_sostav,
            //select_obj: null,
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
                        if (data.id_sostav == table_arrival_uz.select.id_sostav) {
                            $(row).addClass('selected');
                        }
                    },
                    columns: [
                        { data: "index", title: "Состав" },
                        { data: "dt_inp_station", title: "Дата и время" },
                        { data: "cars", title: "Кол. ваг.", width: "30px" },
                    ],
                });
                this.html_div_table = $('DIV#table-list-arrival-uz_wrapper');
                this.initEventSelect();
                //this.obj_table.hide();
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
                            //table_arrival_uz.getSelectObj(arrival_uz.select);
                            //cars.viewTable(arrival_uz, false);
                        });
            },
            setSelectSostav: function (id) {
                var sostav = getObjects(table_arrival_uz.list, 'id_sostav', id)
                if (sostav != null && sostav.length > 0) {
                    table_arrival_uz.select = sostav[0];
                }
            },
            //getSelectObj: function (select) {
            //    var obj = getObjects(arrival_uz.list, 'IDSostav', select);
            //    if (obj != null) {
            //        arrival_uz.select_obj = obj[0];
            //    }
            //},
            clearSelect: function () {
                this.html_table.find('tbody tr').removeClass('selected');
            },
            loadData: function (data) {
                this.list = data;
                this.obj.clear();
                for (i = 0; i < data.length; i++) {
                    // Добавим данные о станциях
                    this.obj.row.add({
                        "id_sostav": data[i].id_sostav,
                        "index": data[i].index,
                        "dt_inp_station": data[i].dt_inp_station,
                        "cars": data[i].cars,
                    });
                };
                this.obj.draw();
            },
            //enableTable: function (length) {
            //    if (length > 0) {
            //        this.obj_table.show();
            //    } else {
            //        this.obj_table.hide();
            //    }
            //},
            viewTable: function (station_id, data_refresh, callback) {

                if (this.list == null | this.station_id != station_id | data_refresh == true) {
                    // Обновим данные
                    this.station_id = station_id;
                    getAsyncArrivalSostavOfStationUZ(
                        station_id,
                        function (result) {
                            table_arrival_uz.loadData(result);
                            //table_arrival_uz.enableTable(result.length);
                            //cars.viewTable(arrival_uz, false);
                            if (typeof callback === 'function') {
                                callback(result.length);
                            }
                        }
                        );
                } else {
                    //this.enableTable(this.list.length);
                    //cars.viewTable(arrival_uz, false);
                    if (typeof callback === 'function') {
                        callback(this.list.length);
                    }
                };
            },
            //clearListSelect: function () {
            //    this.list = null;
            //    this.select = null;
            //    this.select_obj = null;
            //}
        },
        // группа список прибытия по УЗ
        group_list_arrival_uz = {
            html_div: $("#group-list-arrival-uz"),
            hide: function () {
                this.html_div.hide();
            },
            show: function (id_station, refresh) {
                this.html_div.show();
                table_arrival_uz.viewTable(16, false, null);
            },

        },
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
            code_uz: null
        },
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

                    // определим горловину по умолчанию
                    rw_side.setSideOfStation(rw_stations.select_station.default_side);
                    // Показать панели групп
                    tab_group_list.activeOfStation(rw_stations.select_station);

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
                    //
                    //cars.sortPosition();
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
    resurses.initObject("../Scripts/RW/awas.json",
        function () {
        // Загружаем дальше
        tab_group_list.initObject();        // Панель переключения групп
        rw_stations.initObject(true);       // Выбор станций системы RailWay
        rw_side.initObject(true);           // Выбор горловины станций системы RailWay

        table_arrival_uz.initTable(); // таблица прибытия на станции УЗ
    }); // локализация


});

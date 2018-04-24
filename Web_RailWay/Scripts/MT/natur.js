$(function () {
    //-----------------------------------------------------------------------------------------
    // Объявление глобальных переменных
    //-----------------------------------------------------------------------------------------
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
            //-------------------------------
    // Таблица всех маршрутов
    table_sostav_arrival = {
        html_table: $('#table-sostav-arrival'),
        obj_table: null,
        obj: null,
        list: null,
        initObject: function () {
            this.obj = this.html_table.DataTable({
                "paging": false,
                "ordering": false,
                "info": false,
                "autoWidth": false,
                //"scrollY": "600px",
                //"scrollX": true,
                language: {
                    emptyTable: resurses.getText("table_message_emptyTable"),
                    emptyTable: resurses.getText("table_decimal"),
                    emptyTable: resurses.getText("table_message_search"),
                },
                jQueryUI: false,
                "createdRow": function (row, data, index) {
                },
                columns: [
                    { data: "year", title: resurses.getText("table_field_year"), width: "50px", orderable: false, searchable: false },
                    { data: "month", title: resurses.getText("table_field_month"), width: "50px", orderable: false, searchable: false },
                    { data: "count", title: resurses.getText("table_field_count_xml"), width: "50px", orderable: false, searchable: false },
                    { data: "start_dt", title: resurses.getText("table_field_start_dt"), width: "150px", orderable: false, searchable: false },
                    { data: "stop_dt", title: resurses.getText("table_field_stop_dt"), width: "150px", orderable: false, searchable: false },
                ],

            });
            this.obj_table = $('DIV#table-sostav-arrival_wrapper');
            $('#table-sostav-arrival_filter').hide();   // спрятать фильтр
        },
        viewTable: function (data_refresh) {
            if (this.list == null | data_refresh == true) {
                // Обновим данные
                getAsyncCountArrivalNaturList(
                    function (result) {
                        table_sostav_arrival.list = result;
                        table_sostav_arrival.loadDataTable(result);
                        table_sostav_arrival.obj.draw();
                    }
                    );
            } else {
                table_sostav_arrival.loadDataTable(this.list);
                table_sostav_arrival.obj.draw();
            };
        },
        loadDataTable: function (data) {
            this.list = data;
            this.obj.clear();
            for (i = 0; i < data.length; i++) {
                this.obj.row.add({
                    "year": data[i].year,
                    "month": data[i].month,
                    "count": data[i].count,
                    "start_dt": data[i].start_dt,
                    "stop_dt": data[i].stop_dt
                });
            }
        },
    },
    // Таблица всех маршрутов
    table_sostav_approaches = {
        html_table: $('#table-sostav-approaches'),
        obj_table: null,
        obj: null,
        list: null,
        initObject: function () {
            this.obj = this.html_table.DataTable({
                "paging": false,
                "ordering": false,
                "info": false,
                "autoWidth": false,
                //"scrollY": "600px",
                //"scrollX": true,
                language: {
                    emptyTable: resurses.getText("table_message_emptyTable"),
                    emptyTable: resurses.getText("table_decimal"),
                    emptyTable: resurses.getText("table_message_search"),
                },
                jQueryUI: false,
                "createdRow": function (row, data, index) {
                },
                columns: [
                    { data: "year", title: resurses.getText("table_field_year"), width: "50px", orderable: false, searchable: false },
                    { data: "month", title: resurses.getText("table_field_month"), width: "50px", orderable: false, searchable: false },
                    { data: "count", title: resurses.getText("table_field_count_xml"), width: "50px", orderable: false, searchable: false },
                    { data: "start_dt", title: resurses.getText("table_field_start_dt"), width: "150px", orderable: false, searchable: false },
                    { data: "stop_dt", title: resurses.getText("table_field_stop_dt"), width: "150px", orderable: false, searchable: false },
                ],

            });
            this.obj_table = $('DIV#table-sostav-approaches_wrapper');
            $('#table-sostav-approaches_filter').hide();   // спрятать фильтр
        },
        viewTable: function (data_refresh) {
            if (this.list == null | data_refresh == true) {
                // Обновим данные
                getAsyncCountApproachesNaturList(
                    function (result) {
                        table_sostav_approaches.list = result;
                        table_sostav_approaches.loadDataTable(result);
                        table_sostav_approaches.obj.draw();
                    }
                    );
            } else {
                table_sostav_approaches.loadDataTable(this.list);
                table_sostav_approaches.obj.draw();
            };
        },
        loadDataTable: function (data) {
            this.list = data;
            this.obj.clear();
            for (i = 0; i < data.length; i++) {
                this.obj.row.add({
                    "year": data[i].year,
                    "month": data[i].month,
                    "count": data[i].count,
                    "start_dt": data[i].start_dt,
                    "stop_dt": data[i].stop_dt
                });
            }
        },
    }
    //-----------------------------------------------------------------------------------------
    // Функции
    //-----------------------------------------------------------------------------------------

    //-----------------------------------------------------------------------------------------
    // Инициализация объектов
    //-----------------------------------------------------------------------------------------
    resurses.initObject("/railway/Scripts/MT/mt.json",
    function () {
        //// Загружаем дальше
        $('#label-sostav-arrival').text(resurses.getText("label_sostav_arrival"));
        $('#label-sostav-approaches').text(resurses.getText("label_sostav_approaches"));
        table_sostav_arrival.initObject();
        table_sostav_approaches.initObject();
        table_sostav_arrival.viewTable(true);
        table_sostav_approaches.viewTable(true);
    }); // локализация

});
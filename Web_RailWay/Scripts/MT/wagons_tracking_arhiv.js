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
        // class wtReport
        wtReport = {
            id: -1,
            Report: null,
            Description: null
        }
    // Объект группа вагонов для отчета
    wt_report_cars = {
        html_select: $('select[name ="station"]'),
        list_reports: null,            // Список отчетов (список вагонов)
        select_report: wtReport,              // Выбранный отчет (список вагонов)
        // Инициализировать объект
        initObject: function (select) {
            getAsyncWTReports(function (result) {
                wt_report_cars.list_reports = result;
                if (select) {
                    wt_report_cars.initHtmlSelect(result);
                }
            });
        },
        // Инициализировать html компонент select
        initHtmlSelect: function (data) {
            initSelect(
                wt_report_cars.html_select,
                { width: 350 },
                data,
                function (row) {
                    return { value: row.id, text: row.Report };
                },
                wt_report_cars.select_report.id,
                function (event, ui) {
                    event.preventDefault();
                    var id = Number(ui.item.value);
                    // Должны выбрать станцию
                    if (id > 0) {
                        wt_report_cars.setSelect(id);
                        // показать отчет
                        tab_group_reports.activeTable(tab_group_reports.active, true);
                    }
                },
            null);
        },
        // Получить выбраный отчет (class)
        setSelect: function (id) {
            var report = getObjects(wt_report_cars.list_reports, 'id', id)
            if (report != null && report.length > 0) {
                wt_report_cars.select_report = report[0];
            }
        }
    },
    // Группы отчотов
    tab_group_reports = {
        html_div: $("#tabs-report"),
        active: 0,
        initObject: function () {
            $('#link-tabs-1').text(resurses.getText("link_tabs_1"));
            $('#link-tabs-2').text(resurses.getText("link_tabs_2"));
            //$('#link-tabs-3').text(resurses.getText("link_tabs_3"));
            //$('#link-tabs-4').text(resurses.getText("link_tabs_4"));
            //$('#link-tabs-5').text(resurses.getText("link_tabs_5"));
            this.html_div.tabs({
                collapsible: true,
                activate: function (event, ui) {
                    tab_group_reports.active = tab_group_reports.html_div.tabs("option", "active");
                    tab_group_reports.activeTable(tab_group_reports.active, false);
                },
            });
            //tab_group_reports.activeTable(tab_group_reports.active, false);
        },
        activeTable: function (active, data_refresh) {
            if (active == 0) {
                table_wt.viewTable(data_refresh);
            }
            if (active == 1) {
                tab_type_routes.activeTable(tab_type_routes.active, false);
            }
        }
    },
    // Типы отчетов
    tab_type_routes = {
        html_div: $("#tabs-report-routes"),
        html_panel_select_div: $("#panel-select-routes"),
        label_select_date: $('<label>' + (lang == 'en' ? "Select period:" : "Выберите период:") + '</label>'),
        span_select_range: $('<span id="select-range-last"></span>'),
        label_to: $('<label>' + (lang == 'en' ? "to" : "до") + '</label>'),
        input_data_start: $('<input id="date-start-last" name="date_start-last" size="20">'),
        input_data_stop: $('<input id="date-stop-last" name="date-stop-last" size="20">'),
        button_excel: $('<button class="ui-button ui-widget ui-corner-all">' + (lang == 'en' ? "Excel" : "Excel") + '</button>'),
        obj_range: null,
        start: null,
        stop: null,
        active: 0,
        initObject: function () {
            $('#link-tabs-routes-1').text(resurses.getText("link_tabs_routes_1"));
            $('#link-tabs-routes-2').text(resurses.getText("link_tabs_routes_2"));
            $('#link-tabs-routes-3').text(resurses.getText("link_tabs_routes_3"));
            // Настроим панель
            tab_type_routes.span_select_range
                .append(tab_type_routes.input_data_start)
                .append(tab_type_routes.label_to)
                .append(tab_type_routes.input_data_stop);
            tab_type_routes.html_panel_select_div
                .append(tab_type_routes.button_excel)
                .append(tab_type_routes.label_select_date)
                .append(tab_type_routes.span_select_range);
            // настроим компонент выбора времени
            tab_type_routes.obj_range = $('#select-range-last').dateRangePicker(
                {
                    startOfWeek: 'monday',
                    separator: lang == 'en' ? 'to' : 'по',
                    language: lang,
                    format: lang == 'en' ? 'MM/DD/YYYY HH:mm' : 'DD.MM.YYYY HH:mm',
                    autoClose: false,
                    showShortcuts: false,
                    getValue: function () {
                        if ($('#date-start').val() && $('#date-stop-last').val())
                            return $('#date-start-last').val() + ' to ' + $('#date-stop-last').val();
                        else
                            return '';
                    },
                    setValue: function (s, s1, s2) {
                        $('#date-start-last').val(s1);
                        $('#date-stop-last').val(s2);
                    },
                    time: {
                        enabled: true
                    },
                }).
                bind('datepicker-change', function (evt, obj) {
                    tab_type_routes.start = obj.date1;
                    tab_type_routes.stop = obj.date2;
                })
                .bind('datepicker-closed', function () {
                    //var trs = $('tr.shown');
                    //for (i = 0; i < trs.length; i++) {
                    //    var row = table_wt_last.obj.row(trs[i]);
                    //    if (row.child.isShown()) {
                    //        table_wt_last.loadChildDataTable(row.data());
                    //    }
                    //}
                    //table_wt_last.viewTable(true);
                    tab_type_routes.activeTable(tab_type_routes.active, true);
                });
            var dt = new Date();
            tab_type_routes.start = new Date(dt.getFullYear(), 00, 01, 00, 00, 00);
            tab_type_routes.stop = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 23, 59, 59);
            var s_d_start = tab_type_routes.start.getDate() + '.' + (tab_type_routes.start.getMonth() + 1) + '.' + tab_type_routes.start.getFullYear() + ' ' + tab_type_routes.start.getHours() + ':' + tab_type_routes.start.getMinutes();
            var s_d_stop = tab_type_routes.stop.getDate() + '.' + (tab_type_routes.stop.getMonth() + 1) + '.' + tab_type_routes.stop.getFullYear() + ' ' + tab_type_routes.stop.getHours() + ':' + tab_type_routes.stop.getMinutes()
            tab_type_routes.obj_range.data('dateRangePicker').setDateRange(s_d_start, s_d_stop, true);
            this.html_div.tabs({
                collapsible: true,
                activate: function (event, ui) {
                    tab_type_routes.active = tab_type_routes.html_div.tabs("option", "active");
                    tab_type_routes.activeTable(tab_type_routes.active, false);
                },
            });
            //tab_group_reports.activeTable(tab_group_reports.active, false);
        },
        activeTable: function (active, data_refresh) {
            if (active == 0) {

            }
            if (active == 1) {
                table_wt_last.viewTable(data_refresh);
            }
        }
    },

    //-------------------------------
    // Таблица детального расположения вагонов
    table_wt_last = {
        html_table: $('#table-list-wt-last'),
        html_div_panel: $('<div class="dt-buttons setup-operation" id="property"></div>'),
        label_last_date: $('<label>' + (lang == 'en' ? "Actual on: " : "Актуально на: ") + '</label>'),
        label_last_date_value: $('<label id="label-last-date-value"></label>'),
        label_last_total: $('<label>' + (lang == 'en' ? "Total number of wagons: " : "Общее количество вагонов: ") + '</label>'),
        label_last_total_value: $('<label id="label-last-total-value"></label>'),
        obj_table: null,
        obj: null,
        list: null,
        last_date: null,
        report_id: null,
        initObject: function () {
            this.obj = this.html_table.DataTable({
                "paging": false,
                "ordering": false,
                "info": false,
                language: {
                    decimal: lang == 'en' ? "." : ",",
                    search: lang == 'en' ? "Search" : "Найти вагон:",
                },
                jQueryUI: true,
                "createdRow": function (row, data, index) {
                    if (data.count_surplus_1 != null) {
                        $('td', row).eq(2).addClass('not-limit');
                    }
                    if (data.count_surplus_2 != null) {
                        $('td', row).eq(7).addClass('not-limit');
                    }
                    if (data.count_surplus_3 != null) {
                        $('td', row).eq(12).addClass('not-limit');
                    }
                    if (data.count_surplus_4 != null) {
                        $('td', row).eq(17).addClass('not-limit');
                    }
                    if (data.count_norma_1 != null) {
                        $('td', row).eq(3).addClass('ok-limit');
                    }
                    if (data.count_norma_2 != null) {
                        $('td', row).eq(8).addClass('ok-limit');
                    }
                    if (data.count_norma_3 != null) {
                        $('td', row).eq(13).addClass('ok-limit');
                    }
                    if (data.count_norma_4 != null) {
                        $('td', row).eq(18).addClass('ok-limit');
                    }
                },
                "footerCallback": function (row, data, start, end, display) {

                    var api = this.api(), data;

                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    // Вернуть среднее значение
                    var avg = function (data) {
                        var count = 0;
                        var sum = 0;
                        for (i = 0; i < data.length; i++) {
                            sum += data[i];
                            if (data[i] != null) {
                                count++;
                            }
                        }
                        return sum!= 0 | count!=0 ? sum / count : 0;
                    }
                    // Total over all pages
                    total11 = api.column(1).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total12 = api.column(2).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total13 = api.column(3).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total14 = api.column(4).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    //total15 = api.column(5).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total15 = avg(api.column(5).data());
                    //data_avg1 = api.column(5).data();

                    total21 = api.column(6).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total22 = api.column(7).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total23 = api.column(8).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total24 = api.column(9).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total25 = avg(api.column(10).data());

                    total31 = api.column(11).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total32 = api.column(12).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total33 = api.column(13).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total34 = api.column(14).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total35 = avg(api.column(15).data());

                    total41 = api.column(16).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total42 = api.column(17).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total43 = api.column(18).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total44 = api.column(19).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total45 = avg(api.column(20).data());
                    // Update footer
                    $(api.column(1).footer()).html(total11);
                    $(api.column(2).footer()).html(total12);
                    $(api.column(3).footer()).html(total13);
                    $(api.column(4).footer()).html(total14);
                    $(api.column(5).footer()).html(total15.toFixed(2));

                    $(api.column(6).footer()).html(total21);
                    $(api.column(7).footer()).html(total22);
                    $(api.column(8).footer()).html(total23);
                    $(api.column(9).footer()).html(total24);
                    $(api.column(10).footer()).html(total25.toFixed(2));

                    $(api.column(11).footer()).html(total31);
                    $(api.column(12).footer()).html(total32);
                    $(api.column(13).footer()).html(total33);
                    $(api.column(14).footer()).html(total34);
                    $(api.column(15).footer()).html(total35.toFixed(2));

                    $(api.column(16).footer()).html(total41);
                    $(api.column(17).footer()).html(total42);
                    $(api.column(18).footer()).html(total43);
                    $(api.column(19).footer()).html(total44);
                    $(api.column(20).footer()).html(total45.toFixed(2));
                    table_wt_last.label_last_total_value.text(total11 + total21 + total31 + total41);
                },
                columns: [
                    { data: "name_station", title: "Станция назначения", width: "150px", orderable: false, searchable: false },
                    { data: "count_1", title: "Всего", width: "50px", orderable: false, searchable: false, },
                    { data: "count_surplus_1", title: "С привыш. нормат. времени", width: "50px", orderable: false, searchable: false },
                    { data: "count_norma_1", title: "В нормат.", width: "50px", orderable: false, searchable: false },
                    { data: "time_limit_1", title: "Норматив", width: "50px", orderable: false, searchable: false },
                    { data: "time_avg_1", title: "Факт. сред..", width: "50px", orderable: false, searchable: false },
                    { data: "count_2", title: "Всего", width: "50px", orderable: false, searchable: false, },
                    { data: "count_surplus_2", title: "С привыш. нормат. времени", width: "50px", orderable: false, searchable: false },
                    { data: "count_norma_2", title: "В нормат.", width: "50px", orderable: false, searchable: false },
                    { data: "time_limit_2", title: "Норматив", width: "50px", orderable: false, searchable: false },
                    { data: "time_avg_2", title: "Факт. сред..", width: "50px", orderable: false, searchable: false },
                    { data: "count_3", title: "Всего", width: "50px", orderable: false, searchable: false, },
                    { data: "count_surplus_3", title: "С привыш. нормат. времени", width: "50px", orderable: false, searchable: false },
                    { data: "count_norma_3", title: "В нормат.", width: "50px", orderable: false, searchable: false },
                    { data: "time_limit_3", title: "Норматив", width: "50px", orderable: false, searchable: false },
                    { data: "time_avg_3", title: "Факт. сред..", width: "50px", orderable: false, searchable: false },
                    { data: "count_4", title: "Всего", width: "50px", orderable: false, searchable: false, },
                    { data: "count_surplus_4", title: "С привыш. нормат. времени", width: "50px", orderable: false, searchable: false },
                    { data: "count_norma_4", title: "В нормат.", width: "50px", orderable: false, searchable: false },
                    { data: "time_limit_4", title: "Норматив", width: "50px", orderable: false, searchable: false },
                    { data: "time_avg_4", title: "Факт. сред..", width: "50px", orderable: false, searchable: false },
                ],
            });
            this.obj_table = $('DIV#table-list-wt-last_wrapper');
            this.html_div_panel
                .append(this.label_last_date)
                .append(this.label_last_date_value)
                .append(this.label_last_total)
                .append(this.label_last_total_value);
            this.obj_table.prepend(this.html_div_panel);
        },
        viewTable: function (data_refresh) {
            if (wt_report_cars.select_report.id == -1) return;
            //var date = 
            if (this.list == null | data_refresh == true | this.report_id != wt_report_cars.select_report.id) {
                // Обновим данные
                getAsyncLastWagonTrackingAndDateTimeOfReports(
                    (wt_report_cars.select_report.id != null ? wt_report_cars.select_report.id : 0),
                    tab_type_routes.start,
                    tab_type_routes.stop,
                    function (result) {
                        table_wt_last.list = result.list;
                        table_wt_last.last_date = result.dt_last;
                        table_wt_last.report_id = wt_report_cars.select_report.id;
                        table_wt_last.label_last_date_value.text(result.dt_last)
                        table_wt_last.loadDataTable(result.list);
                        table_wt_last.obj.draw();
                    }
                    );
            } else {
                table_wt_last.loadDataTable(this.list);
                table_wt_last.obj.draw();
            };
        },
        loadDataTable: function (data) {
            this.list = data;
            this.obj.clear();
            for (i = 0; i < data.length; i++) {
                this.obj.row.add({
                    "name_station": data[i].name_station,
                    "count_1": data[i].count_1,
                    "count_surplus_1": data[i].count_surplus_1,
                    "count_norma_1": data[i].count_norma_1,
                    "time_limit_1": data[i].time_limit_1,
                    "time_avg_1": data[i].time_avg_1,
                    "count_2": data[i].count_2,
                    "count_surplus_2": data[i].count_surplus_2,
                    "count_norma_2": data[i].count_norma_2,
                    "time_limit_2": data[i].time_limit_2,
                    "time_avg_2": data[i].time_avg_2,
                    "count_3": data[i].count_3,
                    "count_surplus_3": data[i].count_surplus_3,
                    "count_norma_3": data[i].count_norma_3,
                    "time_limit_3": data[i].time_limit_3,
                    "time_avg_3": data[i].time_avg_3,
                    "count_4": data[i].count_4,
                    "count_surplus_4": data[i].count_surplus_4,
                    "count_norma_4": data[i].count_norma_4,
                    "time_limit_4": data[i].time_limit_3,
                    "time_avg_4": data[i].time_avg_4,
                });
            }
            //this.obj.draw();
        },

    }
    //-------------------------------
    // Таблица детального расположения вагонов
    table_wt = {
        data_range: {
            obj: null,
            start: null,
            stop: null,
            label_select_date: $('<label>' + (lang == 'en' ? "Select period:" : "Выберите период:") + '</label>'),
            span_select_range: $('<span id="select-range"></span>'),
            label_to: $('<label>' + (lang == 'en' ? "to" : "до") + '</label>'),
            input_data_start: $('<input id="date-start" name="date_start" size="20">'),
            input_data_stop: $('<input id="date-stop" name="date-stop" size="20">'),
            initObject: function () {
                table_wt.data_range.span_select_range
                .append(table_wt.data_range.input_data_start)
                .append(table_wt.data_range.label_to)
                .append(table_wt.data_range.input_data_stop);
                table_wt.html_div_panel
                    .append(table_wt.data_range.label_select_date)
                    .append(table_wt.data_range.span_select_range);
                table_wt.data_range.obj = $('#select-range').dateRangePicker(
                    {
                        startOfWeek: 'monday',
                        separator: lang == 'en' ? 'to' : 'по',
                        language: lang,
                        format: lang == 'en' ? 'MM/DD/YYYY HH:mm' : 'DD.MM.YYYY HH:mm',
                        autoClose: false,
                        showShortcuts: false,
                        getValue: function () {
                            if ($('#date-start').val() && $('#date-stop').val())
                                return $('#date-start').val() + ' to ' + $('#date-stop').val();
                            else
                                return '';
                        },
                        setValue: function (s, s1, s2) {
                            $('#date-start').val(s1);
                            $('#date-stop').val(s2);
                        },
                        time: {
                            enabled: true
                        },
                    }).
                    bind('datepicker-change', function (evt, obj) {
                        table_wt.data_range.start = obj.date1;
                        table_wt.data_range.stop = obj.date2;
                    })
                    .bind('datepicker-closed', function () {
                        var trs = $('tr.shown');
                        for (i = 0; i < trs.length; i++) {
                            var row = table_wt.obj.row(trs[i]);
                            if (row.child.isShown()) {
                                table_wt.loadChildDataTable(row.data());
                            }
                        }

                    });
                // задать время
                var dt = new Date();
                table_wt.data_range.start = new Date(dt.getFullYear(), dt.getMonth(), (dt.getDate() - 2), dt.getHours(), 00, 00);
                table_wt.data_range.stop = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 23, 59, 59);
                var s_d_start = table_wt.data_range.start.getDate() + '.' + (table_wt.data_range.start.getMonth() + 1) + '.' + table_wt.data_range.start.getFullYear() + ' ' + table_wt.data_range.start.getHours() + ':' + table_wt.data_range.start.getMinutes();
                var s_d_stop = table_wt.data_range.stop.getDate() + '.' + (table_wt.data_range.stop.getMonth() + 1) + '.' + table_wt.data_range.stop.getFullYear() + ' ' + table_wt.data_range.stop.getHours() + ':' + table_wt.data_range.stop.getMinutes()
                table_wt.data_range.obj.data('dateRangePicker').setDateRange(s_d_start, s_d_stop, true);
            }
        },


        html_table: $('#table-list-wagons-tracking'),
        html_div_panel: $('<div class="dt-buttons setup-operation" id="property"></div>'),
        button_close_detali: $('<button class="ui-button ui-widget ui-corner-all">' + (lang == 'en' ? "Close Detali" : "Закрыть детали") + '</button>'),
        button_excel: $('<button class="ui-button ui-widget ui-corner-all">' + (lang == 'en' ? "Excel" : "Excel") + '</button>'),
        obj_table: null,
        obj: null,
        list: null,
        initObject: function () {
            this.obj = this.html_table.DataTable({
                "lengthMenu": [10, 25, 50, 100, 200, 400],
                "paging": true,
                "ordering": true,
                "info": false,
                "select": true,
                //"filter": true,
                //"scrollY": "600px",
                "scrollX": true,
                language: {
                    decimal: lang == 'en' ? "." : ",",
                    search: lang == 'en' ? "Search" : "Найти вагон:",
                },
                jQueryUI: true,
                "createdRow": function (row, data, index) {
                    $(row).attr('id', data.nvagon);
                    //$('td', row).eq(0).html('<a id=' + data.nvagon + ' name="nvagon" href="#">' + data.nvagon + '</a>')
                    //if (data.st_form != null & data.nsost != undefined & data.st_nazn != null & data.nsost != "000") {
                    //    $('td', row).eq(6).html(data.st_form + '-' + data.nsost + '-' + data.st_nazn);
                    //};
                    if (data.id_oper == this.select) {
                        //$(row).addClass('selected');
                    }
                },
                columns: [
                    {
                        className: 'details-control',
                        orderable: false,
                        data: null,
                        defaultContent: '',
                        searchable: false
                    },
                    { data: "nvagon", title: "ВАГОН", orderable: true, searchable: true },
                    { data: "type_car", title: "ТИП", orderable: true, searchable: true },
                    { data: "nameop", title: "ОПЕР.", orderable: true, searchable: true },
                    { data: "dt", title: "ДАТА/ВРЕМЯ", orderable: true, searchable: false },
                    { data: "st_disl", title: "КСДТ", orderable: true, searchable: false },
                    { data: "nst_disl", title: "ДИСЛОКАЦИЯ", width: "150px", orderable: true, searchable: true },
                    { data: "index", title: "ИНДЕКСПОЕЗДА", orderable: true, searchable: false },
                    { data: "st_nazn", title: "КСТН", orderable: true, searchable: false },
                    { data: "nst_nazn", title: "НАЗНАЧЕНИЕ", width: "150px", orderable: true, searchable: true },
                    { data: "kgrp", title: "КГРП", orderable: true, searchable: false },
                    { data: "st_form", title: "КСТО", orderable: true, searchable: false },
                    { data: "nst_form", title: "ОТПРАВЛЕНИЯ", width: "150px", orderable: true, searchable: true },
                    { data: "ves", title: "ВЕС", orderable: true, searchable: false },
                    { data: "kgr", title: "КГР", orderable: true, searchable: false },
                    { data: "type_cargo", title: "ГРУЗ", orderable: true, searchable: true },
                    { data: "ntrain", title: "ПОЕЗД", orderable: true, searchable: false },
                ],
            });
            this.obj_table = $('DIV#table-list-wagons-tracking_wrapper');


            this.html_div_panel
                .append(this.button_close_detali)
                .append(this.button_excel);
            this.obj_table.prepend(this.html_div_panel);

            this.data_range.initObject();

            this.initEventSelect();
            // Обработка события закрыть детали
            table_wt.button_close_detali.on('click', function () {
                var trs = $('tr.shown');

                for (i = 0; i < trs.length; i++) {
                    var row = table_wt.obj.row(trs[i]);
                    if (row.child.isShown()) {
                        // This row is already open - close it
                        row.child.hide();
                    }
                }
                $('tr').removeClass('shown');
            });

            table_wt.button_excel.on('click', function () {
                var data = table_wt.list;
                if (data == null) return;
                var list_tr = '<tr>' +
                    '<th>id</th>' +
                    '<th>nvagon</th>' +
                    '<th>type_car_ru</th>' +
                    '<th>type_car_en</th>' +
                    '<th>dt</th>' +
                    '<th>st_disl</th>' +
                    '<th>nst_disl</th>' +
                    '<th>kodop</th>' +
                    '<th>nameop</th>' +
                    '<th>full_nameop</th>' +
                    '<th>st_form</th>' +
                    '<th>nst_form</th>' +
                    '<th>idsost</th>' +
                    '<th>nsost</th>' +
                    '<th>index</th>' +
                    '<th>st_nazn</th>' +
                    '<th>nst_nazn</th>' +
                    '<th>ntrain</th>' +
                    '<th>st_end</th>' +
                    '<th>nst_end</th>' +
                    '<th>kgr</th>' +
                    '<th>nkgr</th>' +
                    '<th>id_cargo</th>' +
                    '<th>cargo_ru</th>' +
                    '<th>cargo_en</th>' +
                    '<th>type_cargo_ru</th>' +
                    '<th>type_cargo_en</th>' +
                    '<th>group_cargo_ru</th>' +
                    '<th>group_cargo_en</th>' +
                    '<th>kgrp</th>' +
                    '<th>ves</th>' +
                    '<th>updated</th>' +
                    '<th>kgro</th>' +
                    '<th>km</th>' +
                    '</tr>';
                for (i = 0; i < data.length; i++) {
                    list_tr += '<tr>' +
                        '<td>' + data[i].id + '</td>' +
                        '<td>' + data[i].nvagon + '</td>' +
                        '<td>' + data[i].type_car_ru + '</td>' +
                        '<td>' + data[i].type_car_en + '</td>' +
                        '<td>' + data[i].dt + '</td>' +
                        '<td>' + data[i].st_disl + '</td>' +
                        '<td>' + data[i].nst_disl + '</td>' +
                        '<td>' + data[i].kodop + '</td>' +
                        '<td>' + data[i].nameop + '</td>' +
                        '<td>' + data[i].full_nameop + '</td>' +
                        '<td>' + data[i].st_form + '</td>' +
                        '<td>' + data[i].nst_form + '</td>' +
                        '<td>' + data[i].idsost + '</td>' +
                        '<td>' + data[i].nsost + '</td>' +
                        '<td>' + data[i].index + '</td>' +
                        '<td>' + data[i].st_nazn + '</td>' +
                        '<td>' + data[i].nst_nazn + '</td>' +
                        '<td>' + data[i].ntrain + '</td>' +
                        '<td>' + data[i].st_end + '</td>' +
                        '<td>' + data[i].nst_end + '</td>' +
                        '<td>' + data[i].kgr + '</td>' +
                        '<td>' + data[i].nkgr + '</td>' +
                        '<td>' + data[i].id_cargo + '</td>' +
                        '<td>' + data[i].cargo_ru + '</td>' +
                        '<td>' + data[i].cargo_en + '</td>' +
                        '<td>' + data[i].type_cargo_ru + '</td>' +
                        '<td>' + data[i].type_cargo_en + '</td>' +
                        '<td>' + data[i].group_cargo_ru + '</td>' +
                        '<td>' + data[i].group_cargo_en + '</td>' +
                        '<td>' + data[i].kgrp + '</td>' +
                        '<td>' + data[i].ves + '</td>' +
                        '<td>' + data[i].updated + '</td>' +
                        '<td>' + data[i].kgro + '</td>' +
                        '<td>' + data[i].km + '</td>' +
                        '</tr>';
                    var table = '<table>' + list_tr + '</table>';
                }
                fnExcelReport(table, "WagonsTracking");
            });

        },
        viewTable: function (data_refresh) {
            if (this.list == null | data_refresh == true) {
                // Обновим данные
                getAsyncDBWagonsTrackingOfReports(
                    (wt_report_cars.select_report.id != null ? wt_report_cars.select_report.id : 0),
                    function (result) {
                        table_wt.list = result;
                        table_wt.loadDataTable(result);
                        table_wt.initComplete();
                        table_wt.obj.draw();
                        //table_ways.enableTable(result.length);
                        //table_cars_details.viewTable(table_ways, false);
                        //if (typeof callback === 'function') {
                        //    callback(result.length);
                        //}
                    }
                    );
            } else {
                table_wt.loadDataTable(this.list);
                table_wt.initComplete();
                table_wt.obj.draw();
                //this.enableTable(this.list.length);
                //table_cars_details.viewTable(table_ways, false);
                //if (typeof callback === 'function') {
                //    callback(this.list.length);
                //}
            };
        },
        loadDataTable: function (data) {
            this.list = data;
            this.obj.clear();
            for (i = 0; i < data.length; i++) {
                this.obj.row.add({

                    "nvagon": data[i].nvagon,
                    "type_car": lang == 'en' ? data[i].type_car_en : data[i].type_car_ru,
                    "nameop": data[i].nameop,
                    "dt": data[i].dt,
                    "st_disl": data[i].st_disl,
                    "nst_disl": data[i].nst_disl,
                    "index": data[i].index,
                    "st_nazn": data[i].st_nazn,
                    "nst_nazn": data[i].nst_nazn,
                    "kgrp": data[i].kgrp,
                    "st_form": data[i].st_form,
                    "nst_form": data[i].nst_form,
                    "ves": data[i].ves,
                    "kgr": data[i].kgr,
                    "type_cargo": lang == 'en' ? data[i].type_cargo_en : data[i].type_cargo_ru,
                    "ntrain": data[i].ntrain,
                    //                        "nsost": data[i].nsost,
                });
            }
            //this.obj.draw();
        },
        initComplete: function () {
            table_wt.obj.columns([15]).every(function () {
                var column = this;
                var name = $(column.header()).attr('title');
                var select = $('<select><option value="">' + (lang == 'en' ? 'All' : 'Все') + '</option></select>')
                    .appendTo($(column.header()).empty().append(name))
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );
                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });
                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
        },
        initEventSelect: function () {
            this.html_table.find('tbody')
            .on('click', 'td.details-control', function () {
                var tr = $(this).closest('tr');
                var row = table_wt.obj.row(tr);
                if (row.child.isShown()) {
                    // This row is already open - close it
                    row.child.hide();
                    tr.removeClass('shown');
                }
                else {
                    // Open this row
                    //row.child(table_wt.formatDetalies(row.data())).show();
                    //var dt = new Date();
                    //var d_start = new Date(dt.getFullYear(), dt.getMonth(), (dt.getDate() - 1));
                    //var d_stop = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 23, 59, 59);
                    row.child('<div id="detali' + row.data().nvagon + '" class="detali-wt"></div>').show();
                    table_wt.loadChildDataTable(row.data());
                    //getAsyncDBCarWagonsTracking(
                    //    row.data().nvagon,
                    //    table_wt.data_range.start,
                    //    table_wt.data_range.stop,
                    //    function (result) {
                    //        //var tag = "DIV#" + row.data().nvagon;
                    //        var target = $("#detali" + row.data().nvagon);
                    //        target.empty();
                    //        var tab = table_wt.formatDetalies(result)
                    //        target.append(tab);
                    //    }
                    //);
                    tr.addClass('shown');
                }
            });
        },
        loadChildDataTable: function (data) {
            getAsyncDBCarWagonsTracking(
            data.nvagon,
            table_wt.data_range.start,
            table_wt.data_range.stop,
            function (result) {
                var target = $("#detali" + data.nvagon);
                target.empty();
                var tab = table_wt.formatDetalies(result)
                target.append(tab);
            }
            );
        },
        formatDetalies: function (data) {
            if (data == null || data.length == 0) {
                return 'За указанный период данных нет'
            }
            var list_tr = '<tr>' +
                '<th>' + resurses.getText("table_field_nameop") + '</th>' +
                '<th>' + resurses.getText("table_field_dt") + '</th>' +
                '<th>' + resurses.getText("table_field_nst_disl") + '</th>' +
                '<th>' + resurses.getText("table_field_index") + '</th>' +
                '<th>' + resurses.getText("table_field_nst_nazn") + '</th>' +
                '<th>' + resurses.getText("table_field_kgrp") + '</th>' +
                '<th>' + resurses.getText("table_field_nst_form") + '</th>' +
                '<th>' + resurses.getText("table_field_ves") + '</th>' +
                '<th>' + resurses.getText("table_field_type_cargo_code") + '</th>' +
            '</tr>';

            for (i = 0; i < data.length; i++) {
                var rod_cargo = lang == 'en' ? data[i].type_cargo_en : data[i].type_cargo_ru;
                var st_dislocation = data[i].st_disl != null ? data[i].nst_disl + '(' + data[i].st_disl + ')' : '';
                var st_naznach = data[i].st_nazn != null ? data[i].nst_nazn + '(' + data[i].st_nazn + ')' : '';
                var st_form = data[i].st_form != null ? data[i].nst_form + '(' + data[i].st_form + ')' : '';
                list_tr += '<tr>' +
                    '<td>' + data[i].nameop + '</td>' +
                    '<td>' + data[i].dt + '</td>' +
                    '<td>' + st_dislocation + '</td>' +
                    '<td>' + data[i].index + '</td>' +
                    '<td>' + st_naznach +
                    '<td>' + data[i].kgrp + '</td>' +
                    '<td>' + st_form +
                    '<td>' + data[i].ves + '</td>' +
                    '<td>' + rod_cargo + '(' + data[i].kgr + ')' + '</td>' +
                    '</tr>';
            }
            return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' + list_tr + '</table>';
        }
    }


    //-----------------------------------------------------------------------------------------
    // Функции
    //-----------------------------------------------------------------------------------------
    function fnExcelReport(tab, name_file) {
        var file_name = name_file + '.xls';
        var tab_text = '<html xmlns:x="urn:schemas-microsoft-com:office:excel">';
        tab_text = tab_text + '<head><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet>';

        tab_text = tab_text + '<x:Name>Test Sheet</x:Name>';

        tab_text = tab_text + '<x:WorksheetOptions><x:Panes></x:Panes></x:WorksheetOptions></x:ExcelWorksheet>';
        tab_text = tab_text + '</x:ExcelWorksheets></x:ExcelWorkbook></xml></head><body>';

        tab_text = tab_text + "<table border='1px'>";
        //var tab = $('#table-list-wagons-tracking').html();
        tab_text = tab_text + tab
        tab_text = tab_text + '</table></body></html>';

        var data_type = 'data:application/vnd.ms-excel';

        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");

        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
            if (window.navigator.msSaveBlob) {
                var blob = new Blob([tab_text], {
                    type: "application/csv;charset=utf-8;"
                });
                navigator.msSaveBlob(blob, file_name);
            }
        } else {
            $('#test').attr('href', data_type + ', ' + encodeURIComponent(tab_text));
            $('#test').attr('download', file_name);
        }
    }
    //-----------------------------------------------------------------------------------------
    // Инициализация объектов
    //-----------------------------------------------------------------------------------------
    resurses.initObject("/railway/Scripts/MT/wagons_tracking.json",
        function () {

            $('#label-select').text(resurses.getText("label_select_text"));
            wt_report_cars.initObject(true);
            // Загружаем дальше
            table_wt.initObject();
            table_wt_last.initObject();
            //table_disl.initObject();
            tab_group_reports.initObject(); // Основная группа
            tab_type_routes.initObject(); // Основные типы маршрутов


        }); // локализация

});
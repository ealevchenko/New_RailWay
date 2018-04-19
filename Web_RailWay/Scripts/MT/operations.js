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
        },
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
                        tab_type_operations.activeTable(tab_type_operations.active, true);
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
    // Типы отчетов
    tab_type_operations = {
        html_div: $("#tabs-report-operations"),
        active: 0,
        initObject: function () {
            $('#link-tabs-operations-1').text(resurses.getText("link_tabs_operations_1"));
            //$('#link-tabs-operations-2').text(resurses.getText("link_tabs_operations_2"));
            //$('#link-tabs-operations-3').text(resurses.getText("link_tabs_operations_3"));
            this.html_div.tabs({
                collapsible: true,
                activate: function (event, ui) {
                    tab_type_operations.active = tab_type_operations.html_div.tabs("option", "active");
                    tab_type_operations.activeTable(tab_type_operations.active, false);
                },
            });
        },
        activeTable: function (active, data_refresh) {
            if (active == 0) {
                table_operations_last.viewTable(data_refresh);
            }
            //if (active == 1) {
            //    table_operations_last.viewTable(data_refresh);
            //}
        }
    },
    panel_operations_last = {
        html_div_panel: $('<div class="dt-buttons setup-operation" id="property"></div>'),
        html_div_panel_info: $('<div class="setup-operation" id="last-info"></div>'),
        html_div_panel_select: $('<div class="setup-operation" id="last-select"></div>'),

        //label_last_date: $('<label class="label-text" for="label-last-date-value">' + (lang == 'en' ? "Actual on: " : "Актуально на: ") + '</label>'),
        //label_last_date_value: $('<label class="value-text" id="label-last-date-value"></label>'),
        label_last_total: $('<label class="label-text" for="label-last-total-value"></label>'),
        label_last_total_value: $('<label class="value-text" id="label-last-total-value"></label>'),

        button_close_detali: $('<button class="ui-button ui-widget ui-corner-all"></button>'),
        label_select_date: $('<label></label>'),
        span_select_range: $('<span id="select-range-operations-last"></span>'),
        label_to: $('<label></label>'),
        input_data_start: $('<input id="date-start-operations-last" name="date_start-operations-last" size="20">'),
        input_data_stop: $('<input id="date-stop-operations-last" name="date-stop-operations-last" size="20">'),
        obj_range: null,
        start: null,
        stop: null,
        initPanel: function (obj) {
            // Настроим панель info
            this.html_div_panel_info
                //.append(this.label_last_date)
                //.append(this.label_last_date_value)
                .append(this.label_last_total.text(resurses.getText("label_total_cars")))
                .append(this.label_last_total_value);
            // Настроим панель select
            this.span_select_range
                .append(this.input_data_start)
                .append(this.label_to.text(resurses.getText("table_message_separator")))
                .append(this.input_data_stop);
            this.html_div_panel_select
                .append(this.button_close_detali.text(resurses.getText("button_close_detali")))
                .append(this.label_select_date.text(resurses.getText("label_select_date_detali")))
                .append(this.span_select_range);
            this.html_div_panel
                .append(this.html_div_panel_info)
                .append(this.html_div_panel_select);
            obj.prepend(this.html_div_panel);
            // Обработка события закрыть детали
            this.button_close_detali.on('click', function () {
                var trs = $('tr.shown');

                for (i = 0; i < trs.length; i++) {
                    var row = table_operations_last.obj.row(trs[i]);
                    if (row.child.isShown()) {
                        // This row is already open - close it
                        row.child.hide();
                    }
                }
                $('tr').removeClass('shown');
            });
            // настроим компонент выбора времени
            this.obj_range = $('#select-range-operations-last').dateRangePicker(
                {
                    startOfWeek: 'monday',
                    separator: resurses.getText("table_message_separator"),
                    language: lang,
                    format: resurses.getText("table_date_format"),
                    autoClose: false,
                    showShortcuts: false,
                    getValue: function () {
                        if ($('#date-start-operations-last').val() && $('#date-stop-operations-last').val())
                            return $('#date-start-operations-last').val() + ' to ' + $('#date-stop-operations-last').val();
                        else
                            return '';
                    },
                    setValue: function (s, s1, s2) {
                        $('#date-start-operations-last').val(s1);
                        $('#date-stop-operations-last').val(s2);
                    },
                    time: {
                        enabled: true
                    },
                }).
                bind('datepicker-change', function (evt, obj) {
                    panel_operations_last.start = obj.date1;
                    panel_operations_last.stop = obj.date2;
                })
                .bind('datepicker-closed', function () {
                    table_operations_last.viewTable(true);
                });
            var dt = new Date();
            panel_operations_last.start = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate() - 1, 00, 00, 00);
            panel_operations_last.stop = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 23, 59, 59);

            var s_d_start = panel_operations_last.start.getDate() + '.' + (panel_operations_last.start.getMonth() + 1) + '.' + panel_operations_last.start.getFullYear() + ' ' + panel_operations_last.start.getHours() + ':' + panel_operations_last.start.getMinutes();
            var s_d_stop = panel_operations_last.stop.getDate() + '.' + (panel_operations_last.stop.getMonth() + 1) + '.' + panel_operations_last.stop.getFullYear() + ' ' + panel_operations_last.stop.getHours() + ':' + panel_operations_last.stop.getMinutes()
            panel_operations_last.obj_range.data('dateRangePicker').setDateRange(s_d_start, s_d_stop, true);
        },
    },
    table_operations_last = {
        html_table: $('#table-list-operations'),
        html_div_panel: $('<div class="dt-buttons setup-operation" id="property"></div>'),
        button_to_excel_detali: $('<button class="ui-button ui-widget ui-corner-all" id=""></button>'),
        obj_table: null,
        obj: null,
        list: null,
        report_id: null,
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
                    emptyTable: resurses.getText("table_message_emptyTable"),
                    emptyTable: resurses.getText("table_decimal"),
                    emptyTable: resurses.getText("table_message_search"),
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
                        searchable: false, width: "30px"
                    },
                    { data: "nvagon", title: resurses.getText("table_field_operation_nvagon"), orderable: true, searchable: true },
                    { data: "type_car", title: resurses.getText("table_field_operation_type_car"), orderable: true, searchable: true },
                    { data: "nameop", title: resurses.getText("table_field_operation_nameop"), orderable: true, searchable: true },
                    { data: "dt", title: resurses.getText("table_field_operation_dt"), orderable: true, searchable: false },
                    { data: "st_disl", title: resurses.getText("table_field_operation_st_disl"), orderable: true, searchable: false },
                    { data: "nst_disl", title: resurses.getText("table_field_operation_nst_disl"), width: "150px", orderable: true, searchable: true },
                    { data: "st_form", title: resurses.getText("table_field_operation_st_form"), orderable: true, searchable: false },
                    { data: "nst_form", title: resurses.getText("table_field_operation_nst_form"), width: "150px", orderable: true, searchable: true },
                    { data: "index", title: resurses.getText("table_field_operation_index"), orderable: true, searchable: false },
                    { data: "st_nazn", title: resurses.getText("table_field_operation_st_nazn"), orderable: true, searchable: false },
                    { data: "nst_nazn", title: resurses.getText("table_field_operation_nst_nazn"), width: "150px", orderable: true, searchable: true },
                    { data: "kgro", title: resurses.getText("table_field_operation_kgro"), orderable: true, searchable: false },
                    { data: "kgrp", title: resurses.getText("table_field_operation_kgrp"), orderable: true, searchable: false },
                    { data: "st_end", title: resurses.getText("table_field_operation_st_end"), orderable: true, searchable: false },
                    { data: "nst_end", title: resurses.getText("table_field_operation_nst_end"), width: "150px", orderable: true, searchable: true },
                    { data: "ves", title: resurses.getText("table_field_operation_ves"), orderable: true, searchable: false },
                    { data: "kgr", title: resurses.getText("table_field_operation_kgr"), orderable: true, searchable: false },
                    { data: "type_cargo", title: resurses.getText("table_field_operation_type_cargo"), orderable: true, searchable: true },
                    { data: "ntrain", title: resurses.getText("table_field_operation_ntrain"), orderable: true, searchable: false },
                ],
            });
            this.obj_table = $('DIV#table-list-operations_wrapper');
            panel_operations_last.initPanel(this.obj_table);
            this.initEventSelectChild();
        },
        viewTable: function (data_refresh) {
            if (wt_report_cars.select_report.id == -1) return;
            //var date = 
            if (this.list == null | data_refresh == true | this.report_id != wt_report_cars.select_report.id) {
                // Обновим данные
                getAsyncLastOperationWagonsTrackingOfCarsReports(
                    (wt_report_cars.select_report.id != null ? wt_report_cars.select_report.id : 0),
                    function (result) {
                        table_operations_last.list = result;
                        table_operations_last.report_id = wt_report_cars.select_report.id;
                        panel_operations_last.label_last_total_value.text(result.length);
                        table_operations_last.loadDataTable(result);
                        table_operations_last.obj.draw();
                    }
                    );
            } else {
                table_operations_last.loadDataTable(this.list);
                table_operations_last.obj.draw();
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
                    "st_form": data[i].st_form,
                    "nst_form": data[i].nst_form,
                    "index": data[i].index,
                    "st_nazn": data[i].st_nazn,
                    "nst_nazn": data[i].nst_nazn,
                    "kgro": data[i].kgro,
                    "kgrp": data[i].kgrp,
                    "st_end": data[i].st_end,
                    "nst_end": data[i].nst_end,
                    "ves": data[i].ves,
                    "kgr": data[i].kgr,
                    "type_cargo": lang == 'en' ? data[i].type_cargo_en : data[i].type_cargo_ru,
                    "ntrain": data[i].ntrain,
                    //                        "nsost": data[i].nsost,
                });
            }
        },
        initEventSelectChild: function () {
            this.html_table.find('tbody')
            .on('click', 'td.details-control', function () {
                var tr = $(this).closest('tr');
                var row = table_operations_last.obj.row(tr);
                if (row.child.isShown()) {
                    // This row is already open - close it
                    row.child.hide();
                    tr.removeClass('shown');
                }
                else {
                    row.child('<div id="detali' + row.data().nvagon + '" class="detali-operation"></div>').show();
                    table_operations_last.viewTableChild(row.data());
                    tr.addClass('shown');
                }
            });
        },
        viewTableChild: function (data) {
            getAsyncOperationWagonsTrackingOfNumCarAndDT(
            data.nvagon,
            panel_operations_last.start,
            panel_operations_last.stop,
            function (result) {
                var target = $("#detali" + data.nvagon);
                target.empty();
                target.append('<button class="ui-button ui-widget ui-corner-all" id="button_to_excel_' + data.nvagon + '">'+resurses.getText("button_to_excel")+'</button>');

                var tab = table_operations_last.createTable(result)
                target.append(tab);
                $('#button_to_excel_' + data.nvagon).on('click', function (evt) {
                    var table = $(this).next();
                    fnExcelReport(table.html(), table[0].id)
                });
            }
            );
        },
        createTable: function (data) {
            if (data == null || data.length == 0) {
                return resurses.getText("table_not_data")
            }

            var outVal = function (i) {
                return i != null ? Number(i) : '';
            };

            var list_tr = '<thead><tr>' +
                '<th style="width:50px">' + resurses.getText("table_field_nameop") + '</th>' +
                '<th>' + resurses.getText("table_field_dt") + '</th>' +
                '<th>' + resurses.getText("table_field_nst_disl") + '</th>' +
                '<th>' + resurses.getText("table_field_nst_nazn") + '</th>' +
                '<th>' + resurses.getText("table_field_index") + '</th>' +
                '<th>' + resurses.getText("table_field_nst_form") + '</th>' +
                '<th style="width:50px">' + resurses.getText("table_field_kgro") + '</th>' +
                '<th>' + resurses.getText("table_field_nst_end") + '</th>' +
                '<th style="width:50px">' + resurses.getText("table_field_kgrp") + '</th>' +
                '<th style="width:50px">' + resurses.getText("table_field_km") + '</th>' +
                '<th style="width:50px">' + resurses.getText("table_field_ves") + '</th>' +
                '<th style="width:50px">' + resurses.getText("table_field_type_cargo_code") + '</th>' +
            '</tr></thead>';
            var nvagon = 0;
            list_tr += '<tbody>';
            for (i = 0; i < data.length; i++) {
                var rod_cargo = lang == 'en' ? data[i].type_cargo_en : data[i].type_cargo_ru;
                var st_dislocation = data[i].st_disl != null ? data[i].nst_disl + '(' + data[i].st_disl + ')' : '';
                var st_naznach = data[i].st_nazn != null ? data[i].nst_nazn + '(' + data[i].st_nazn + ')' : '';
                var st_form = data[i].st_form != null ? data[i].nst_form + '(' + data[i].st_form + ')' : '';
                var st_end = data[i].st_end != null ? data[i].nst_end + '(' + data[i].st_end + ')' : '';
                list_tr += '<tr>' +
                    '<td>' + data[i].nameop + '</td>' +
                    '<td>' + data[i].dt + '</td>' +
                    '<td>' + st_dislocation + '</td>' +
                    '<td>' + st_naznach + '</td>' +
                    '<td>' + data[i].index + '</td>' +
                    '<td>' + st_form + '</td>' +
                    '<td>' + outVal(data[i].kgro) + '</td>' +
                    '<td>' + st_end + '</td>' +
                    '<td>' + outVal(data[i].kgrp) + '</td>' +
                    '<td>' + outVal(data[i].km) + '</td>' +
                    '<td>' + data[i].ves + '</td>' +
                    '<td>' + rod_cargo + '(' + data[i].kgr + ')' + '</td>' +
                    '</tr>';
                nvagon = data[i].nvagon;
            }
            list_tr += '</tbody>';
            return '<table class="table-operation-detali" id="table-detali-' + nvagon + '" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' + list_tr + '</table>';
        },
        toExcel: function () {
            var data = this.list;
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
            fnExcelReport(table, "LastOperation");
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
    resurses.initObject("/railway/Scripts/MT/wagons_tracking.json",
    function () {

        $('#label-select').text(resurses.getText("label_select_text"));
        $('#to-excel').text(resurses.getText("button_to_excel"));
        $('#to-excel').on('click', function () {

            if (tab_type_operations.active == 0) {
                table_operations_last.toExcel();
            };
            //if (tab_type_operations.active == 1) {
            //    fnExcelReport(table_operations_last.html_table.html(), "LastWagonsTracking");
            //};

        });
        wt_report_cars.initObject(true);
        //// Загружаем дальше
        table_operations_last.initObject();
        //table_wt_routes.initObject();
        //table_operations_last.initObject();

        ////table_disl.initObject();
        tab_type_operations.initObject(); // Типы маршрутов


    }); // локализация



});
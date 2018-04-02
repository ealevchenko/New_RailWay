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
        // Группы спиков
        tab_group_reports = {
            html_div: $("#tabs-report"),
            active: 0,
            initObject: function () {
                $('#link-tabs-1').text(resurses.getText("link_tabs_1"));
                $('#link-tabs-2').text(resurses.getText("link_tabs_2"));
                $('#link-tabs-3').text(resurses.getText("link_tabs_3"));
                $('#link-tabs-4').text(resurses.getText("link_tabs_4"));
                $('#link-tabs-5').text(resurses.getText("link_tabs_5"));
                this.html_div.tabs({
                    collapsible: true,
                    activate: function (event, ui) {
                        tab_group_reports.active = tab_group_reports.html_div.tabs("option", "active");
                        wagons_tracking.viewReport(tab_group_reports.active, false);
                    },
                });
                tab_group_reports.activeTable(tab_group_reports.active, false);
            },
            activeTable: function (active, data_refresh) {
                if (active == 0) {
                    table_wt.viewTable(data_refresh);
                }
            }
        },
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

            obj_table: null,
            obj: null,
            list: null,
            initObject: function () {
                this.obj = this.html_table.DataTable({
                    "lengthMenu": [10, 25, 50, 100],
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
                    .append(this.button_close_detali);
                this.obj_table.prepend(this.html_div_panel);

                this.data_range.initObject();

                this.initEventSelect();

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

            },
            viewTable: function (data_refresh) {
                if (this.list == null | data_refresh == true) {
                    // Обновим данные
                    getAsyncDBWagonsTracking(
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
    //-----------------------------------------------------------------------------------------
    // Инициализация объектов
    //-----------------------------------------------------------------------------------------
    resurses.initObject("/railway/Scripts/MT/wagons_tracking.json",
        function () {
            // Загружаем дальше
            table_wt.initObject();
            //table_disl.initObject();
            tab_group_reports.initObject();


        }); // локализация

});
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
                wagons_tracking.viewReport(tab_group_reports.active, false);
            },
        },
        table_wt = {
            html_table: $('#table-list-wagons-tracking'),
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
                        $('td', row).eq(0).html('<a id=' + data.nvagon + ' name="nvagon" href="#">' + data.nvagon + '</a>')
                        //if (data.st_form != null & data.nsost != undefined & data.st_nazn != null & data.nsost != "000") {
                        //    $('td', row).eq(6).html(data.st_form + '-' + data.nsost + '-' + data.st_nazn);
                        //};
                        if (data.id_oper == this.select) {
                            //$(row).addClass('selected');
                        }
                    },
                    columns: [
                        { data: "nvagon", title: "ВАГОН", orderable: true, searchable: true },
                        { data: "type_vagon", title: "ТИП", orderable: true, searchable: true },
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
                        { data: "rod_cargo", title: "ГРУЗ", orderable: true, searchable: true },
                        { data: "ntrain", title: "ПОЕЗД", orderable: true, searchable: false },
                    ],
                });
                this.obj_table = $('DIV#table-list-wagons-tracking_wrapper');
            },
            viewTable: function (data) {
                table_wt.loadDataTable(data);
                table_wt.initComplete();
                table_wt.obj.draw();
            },
            loadDataTable: function (data) {
                this.list = data;
                this.obj.clear();
                for (i = 0; i < data.length; i++) {
                    this.obj.row.add({
                        "nvagon": data[i].nvagon,
                        "type_vagon": data[i].type_vagon,
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
                        "rod_cargo": data[i].rod_cargo,
                        "ntrain": data[i].ntrain,
                        "nsost": data[i].nsost,
                    });
                }
                this.obj.draw();
            },
            initComplete: function () {
                table_wt.obj.columns([14]).every(function () {
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
        },
        table_disl = {
            html_table: $('#table-list-wagons-tracking-disl'),
            obj_table: null,
            obj: null,
            list: null,
            initObject: function () {
                this.obj = this.html_table.DataTable({
                    "paging": false,
                    "columnDefs": [{ "visible": false, "targets": 0 }],
                    "order": [[0, 'asc'],[2, 'asc']],
                    //"displayLength": 25,
                    "drawCallback": function (settings) {
                        var api = this.api();
                        var rows = api.rows({ page: 'current' }).nodes();
                        var last = null;
                        api.column(0, { page: 'current' }).data().each(function (group, i) {
                            if (last !== group) {
                                $(rows).eq(i).before(
                                    '<tr class="group"><td colspan="8">' + group + '</td></tr>'
                                );
                                last = group;
                            }
                        });
                    },

                    ////"lengthMenu": [25, 50, 100, "All"],
                    //"paging": true,
                    //"ordering": true,
                    //"info": false,
                    //"select": true,
                    ////"filter": true,
                    ////"scrollY": "600px",
                    //"scrollX": true,
                    language: {
                        decimal: lang == 'en' ? "." : ",",
                        search: lang == 'en' ? "Search" : "Найти вагон:",
                    },
                    jQueryUI: true,
                    //"createdRow": function (row, data, index) {
                    //    $(row).attr('id', data.nvagon);
                    //    $('td', row).eq(0).html('<a id=' + data.nvagon + ' name="nvagon" href="#">' + data.nvagon + '</a>')
                    //    //if (data.st_form != null & data.nsost != undefined & data.st_nazn != null & data.nsost != "000") {
                    //    //    $('td', row).eq(6).html(data.st_form + '-' + data.nsost + '-' + data.st_nazn);
                    //    //};
                    //    if (data.id_oper == this.select) {
                    //        //$(row).addClass('selected');
                    //    }
                    //},
                    columns: [
                        { data: "nst_disl", title: "ДИСЛОКАЦИЯ", width: "150px", orderable: true, searchable: true },
                        { data: "nameop", title: "ОПЕР.", orderable: true, searchable: true },
                        { data: "dt", title: "ДАТА/ВРЕМЯ", orderable: true, searchable: false },
                        { data: "nst_nazn", title: "НАЗНАЧЕНИЕ", width: "150px", orderable: true, searchable: true },
                        { data: "nst_form", title: "ОТПРАВЛЕНИЯ", width: "150px", orderable: true, searchable: true },
                        { data: "rod_cargo", title: "ГРУЗ", orderable: true, searchable: true },
                        { data: "vagon", title: "ВАГОН", orderable: true, searchable: true },
                        { data: "type_vagon", title: "ТИП", orderable: true, searchable: true },
                        { data: "ves", title: "ВЕС", orderable: true, searchable: false },
                    ],
                });
                this.obj_table = $('DIV#table-list-wagons-tracking-disl_wrapper');
            },
            viewTable: function (data) {
                table_disl.loadDataTable(data);
                table_disl.obj.draw();
                table_disl.orderEvent();
            },
            loadDataTable: function (data) {
                this.list = data;
                this.obj.clear();
                for (i = 0; i < data.length; i++) {
                    this.obj.row.add({
                        "nst_disl": data[i].nst_disl,
                        "nameop": data[i].nameop,
                        "dt": data[i].dt,
                        "nst_nazn": data[i].nst_nazn,
                        "nst_form": data[i].nst_form,
                        "rod_cargo": data[i].rod_cargo,
                        "vagon": "0",
                        "type_vagon": data[i].type_vagon,
                        "ves": "0",
                    });
                }
                this.obj.draw();
            },
            orderEvent: function () {
                table_disl.obj_table.find('tbody').on('click', 'tr.group', function () {
                    var currentOrder = table_disl.obj.order()[0];
                    if (currentOrder[0] === 0 && currentOrder[1] === 'asc') {
                        table_disl.obj.order([0, 'desc']).draw();
                    }
                    else {
                        table_disl.obj.order([0, 'asc']).draw();
                    }

                });
            }
        },
        wagons_tracking = {
            list: null,
            // Загрузить данные
            loadData: function (callback) {
                getAsyncWagonsTracking(function (result) {
                    var list_wt = [];
                    for (var i = 0; i < result.length; i++) {
                        var res = getDefaultReferenceCargo(result[i].kgr);

                        list_wt.push({
                            nvagon: result[i].nvagon,
                            type_vagon: "ПВ",
                            st_disl: result[i].st_disl,
                            nst_disl: result[i].nst_disl,
                            kodop: result[i].kodop,
                            nameop: result[i].nameop,
                            dt: result[i].dt,
                            nst_form: result[i].nst_form,
                            st_form: result[i].st_form,
                            nsost: result[i].nsost,
                            st_nazn: result[i].st_nazn,
                            nst_nazn: result[i].nst_nazn,
                            ntrain: result[i].ntrain,
                            st_end: result[i].st_end,
                            nst_end: result[i].nst_end,
                            idsost: result[i].idsost,
                            kgr: result[i].kgr,
                            nkgr: result[i].nkgr,
                            kgrp: result[i].kgrp,
                            ves: result[i].ves,
                            updated: result[i].updated,
                            full_nameop: result[i].full_nameop,
                            kgro: result[i].kgro,
                            km: result[i].km,
                            rod_cargo: res != null ? res.ReferenceTypeCargo.type_name_ru : null,
                            index: result[i].st_form != null & result[i].nsost != undefined & result[i].st_nazn != null & result[i].nsost != "000" ? result[i].st_form + '-' + result[i].nsost + '-' + result[i].st_nazn : "-"
                        });
                    }
                    wagons_tracking.list = list_wt;
                    if (typeof callback === 'function') {
                        callback(list_wt);
                    }
                });
            },
            // Инициализировать объект
            initObject: function () {
                this.loadData(function (data) {

                });
            },

            //initWagonsTracking: function () {
            //    //getAsyncWagonsTracking(function (result) {
            //    //    var list_wt = [];
            //    //    for (var i = 0; i < result.length; i++) {
            //    //        getAsyncDefaultReferenceCargo(result[i].kgr, function (data) {
            //    //            list_wt.push({
            //    //                nvagon: result[i].nvagon,
            //    //                st_disl: result[i].st_disl,
            //    //                nst_disl: result[i].nst_disl,
            //    //                kodop: result[i].kodop,
            //    //                nameop: result[i].nameop,
            //    //                dt: result[i].dt,
            //    //                nst_form: result[i].nst_form,
            //    //                st_form: result[i].st_form,
            //    //                nsost: result[i].nsost,
            //    //                st_nazn: result[i].st_nazn,
            //    //                nst_nazn: result[i].nst_nazn,
            //    //                ntrain: result[i].ntrain,
            //    //                st_end: result[i].st_end,
            //    //                nst_end: result[i].nst_end,
            //    //                idsost: result[i].idsost,
            //    //                kgr: result[i].kgr,
            //    //                nkgr: result[i].nkgr,
            //    //                kgrp: result[i].kgrp,
            //    //                ves: result[i].ves,
            //    //                updated: result[i].updated,
            //    //                full_nameop: result[i].full_nameop,
            //    //                kgro: result[i].kgro,
            //    //                km: result[i].km,
            //    //                rod_cargo: data
            //    //            });
            //    //        })



            //    //    }
            //    //    wagons_tracking.list = list_wt;
            //    //});
            //},
            // Показать отчет
            viewReport: function (tab, data_refresh) {
                if (this.list == null | data_refresh == true) {
                    // Обновим данные
                    wagons_tracking.loadData(function (data) {
                        wagons_tracking.viewTable(data, tab);
                    });
                } else {
                    // Не обновим данные
                    wagons_tracking.viewTable(wagons_tracking.list, tab)
                }
            },
            viewTable: function (data, tab) {
                if (tab == 0) {
                    table_wt.viewTable(data);
                }
                if (tab == 1) {
                    table_disl.viewTable(data);
                }
            },
        }

    //-----------------------------------------------------------------------------------------
    // Функции
    //-----------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------
    // Инициализация объектов
    //-----------------------------------------------------------------------------------------
    resurses.initObject("../../../Scripts/MT/wagons_tracking.json",
        function () {
            // Загружаем дальше
            table_wt.initObject();
            table_disl.initObject();
            tab_group_reports.initObject();
            wagons_tracking.initObject();
            //wagons_tracking.initTable();
            //wagons_tracking.viewTable(false);

        }); // локализация

});
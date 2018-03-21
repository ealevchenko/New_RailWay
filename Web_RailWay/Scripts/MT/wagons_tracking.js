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

        wagons_tracking = {
            html_table: $('#table-list-wagons-tracking'),
            obj_table: null,
            obj: null,
            list: null,
            loadData: function (callback) {
                getAsyncWagonsTracking(function (result) {
                    var list_wt = [];
                    for (var i = 0; i < result.length; i++) {
                        var res = getDefaultReferenceCargo(result[i].kgr);

                                list_wt.push({
                                    nvagon: result[i].nvagon,
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
                //getAsyncWagonsTracking(function (result) {
                //    wagons_tracking.list = result;
                //    if (typeof callback === 'function') {
                //        callback(result);
                //    }
                //});
            },
            initObject: function () {
                this.initTableReport1();
            },
            initTableReport1: function () {
                this.obj = this.html_table.DataTable({
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
                        { data: "nameop", title: "ОПЕР.", orderable: true, searchable: false },
                        { data: "dt", title: "ДАТА/ВРЕМЯ", orderable: true, searchable: false },
                        { data: "st_disl", title: "КСДТ", orderable: true, searchable: false },
                        { data: "nst_disl", title: "ДИСЛОКАЦИЯ", width: "150px", orderable: true, searchable: false },
                        { data: "index", title: "ИНДЕКСПОЕЗДА", orderable: true, searchable: false },
                        { data: "st_nazn", title: "КСТН", orderable: true, searchable: false },
                        { data: "nst_nazn", title: "НАЗНАЧЕНИЕ", width: "150px", orderable: true, searchable: false },
                        { data: "kgrp", title: "КГРП", orderable: true, searchable: false },
                        { data: "st_form", title: "КСТО", orderable: true, searchable: false },
                        { data: "nst_form", title: "ОТПРАВЛЕНИЯ", width: "150px", orderable: true, searchable: false },
                        { data: "ves", title: "ВЕС", orderable: true, searchable: false },
                        { data: "kgr", title: "КГР", orderable: true, searchable: false },
                        { data: "cargo", title: "ГРУЗ", orderable: true, searchable: false },
                        { data: "ntrain", title: "ПОЕЗД", orderable: true, searchable: false },
                    ],
                });
                this.obj_table = $('DIV#table-list-wagons-tracking_wrapper');
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

            viewReport: function (tab, data_refresh) {
                if (this.list == null | data_refresh == true) {
                    // Обновим данные
                    this.loadData(function (data) {

                        wagons_tracking.viewTable(data, tab);
                        //initComplete: function () {
                        wagons_tracking.obj.columns([2, 5, 8, 11]).every(function (i) {
                            var column = wagons_tracking.obj.column(i);
                            var name = $(column.header()).attr('');
                                var select = $('<select><option value="">' + (lang == 'en' ? 'All' : 'Все') + '</option></select>')
                                    .appendTo($(column.header()).empty().append(name))
                                    .on('change', function () {
                                        var val = $.fn.dataTable.util.escapeRegex(
                                            $(this).val()
                                        );
                                        column.data()
                                            .search(val ? '^' + val + '$' : '', true, false)
                                            .draw();
                                    });
                                column.data().unique().sort().each(function (d, j) {
                                    select.append('<option value="' + d + '">' + d + '</option>')
                                });
                            });
                        //},
                    });
                } else {
                    // Не обновим данные
                    wagons_tracking.viewTable(wagons_tracking.list, tab)
                }
            },
            viewTable: function (data, tab) {

                wagons_tracking.loadDataTable1(data);
            },
            loadDataTable1: function (data) {
                this.list = data;
                this.obj.clear();
                for (i = 0; i < data.length; i++) {
                    this.obj.row.add({
                        "nvagon": data[i].nvagon,
                        "type_vagon": "ПВ",
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
                        //"kgr": data[i].kgr,
                        "cargo": data[i].rod_cargo,
                        "ntrain": data[i].ntrain,
                        "nsost": data[i].nsost,
                    });
                }
                this.obj.draw();
            },
        }

    //-----------------------------------------------------------------------------------------
    // Функции
    //-----------------------------------------------------------------------------------------

    //-----------------------------------------------------------------------------------------
    // Инициализация объектов
    //-----------------------------------------------------------------------------------------
    resurses.initObject("../../Scripts/MT/wagons_tracking.json",
        function () {
            // Загружаем дальше
            tab_group_reports.initObject();
            wagons_tracking.initObject();
            //wagons_tracking.initTable();
            //wagons_tracking.viewTable(false);

        }); // локализация

});
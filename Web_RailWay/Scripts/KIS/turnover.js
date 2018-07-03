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
    dt = new Date(),
    start = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate() - 1, 00, 00, 00),
    stop = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 23, 59, 59),
    datetime_range = {
        html_range: $("#select-range"),
        obj: null,
        initObject: function () {
            this.obj = this.html_range.dateRangePicker(
                {
                    startOfWeek: 'monday',
                    separator: resurses.getText("table_message_separator"),
                    language: lang,
                    format: resurses.getText("table_date_format"),
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
                    start = obj.date1;
                    stop = obj.date2;
                })
                .bind('datepicker-closed', function () {
                    tab_type_turnover.activeTable(tab_type_turnover.active, true);
                });
            var s_d_start = start.getDate() + '.' + (start.getMonth() + 1) + '.' + start.getFullYear() + ' ' + start.getHours() + ':' + start.getMinutes();
            var s_d_stop = stop.getDate() + '.' + (stop.getMonth() + 1) + '.' + stop.getFullYear() + ' ' + stop.getHours() + ':' + stop.getMinutes();
            datetime_range.obj.data('dateRangePicker').setDateRange(s_d_start, s_d_stop, true);
        }
    },
    // Типы отчетов
    tab_type_turnover = {
        html_div: $("#tabs-report-turnover"),
        active: 0,
        initObject: function () {
            $('#link-tabs-turnover-1').text(resurses.getText("link_tabs_turnover_1"));
            //$('#link-tabs-turnover-2').text(resurses.getText("link_tabs_turnover_2"));
            //$('#link-tabs-turnover-3').text(resurses.getText("link_tabs_turnover_3"));
            this.html_div.tabs({
                collapsible: true,
                activate: function (event, ui) {
                    tab_type_turnover.active = tab_type_turnover.html_div.tabs("option", "active");
                    tab_type_turnover.activeTable(tab_type_turnover.active, false);
                },
            });
            this.activeTable(0, true);
        },
        activeTable: function (active, data_refresh) {
            if (active == 0) {
                table_turnover.viewTable(data_refresh);
            }
            //if (active == 1) {

            //}
            //if (active == 2) {

            //}
        }
    },
    // список станций
    rw_stations = {
        list: [],
        initObject: function () {
            getAsyncStations(function (result)
            { rw_stations.list = result; });
        },
        selectStationKIS: function (id_kis) {
            var station = getObjects(this.list, 'id_kis', id_kis)
            if (station != null && station.length > 0) {
                return station[0];
            }
        },
    },
    // список годность (беру из RW)
    rw_car_condition = {
        list: [],
        initObject: function () {
            getAsyncCarConditions(function (result)
            { rw_car_condition.list = result; });
        },
        getСondition: function (id) {
            var condition = getObjects(this.list, 'id', id)
            if (condition != null && condition.length > 0) {
                return condition[0];
            }
        },
    },
    // список цехов получателей груза
    kis_cex = {
        list: [],
        initObject: function () {
            getAsyncPromCex(function (result)
            { kis_cex.list = result; });
        },
        getCex: function (id) {
            var cex = getObjects(this.list, 'K_PODR', id)
            if (cex != null && cex.length > 0) {
                return cex[0];
            }
        },
    },
    // список грузов
    kis_gruz = {
        list: [],
        initObject: function () {
            getAsyncGruzSP(function (result)
            { kis_gruz.list = result; });
        },
        getGruz: function (id) {
            var gruz = getObjects(this.list, 'K_GRUZ', id)
            if (gruz != null && gruz.length > 0) {
                return gruz[0];
            }
        },
    },
    // список собственников
    kis_owner = {
        list: [],
        initObject: function () {
            getAsyncKometaSobstvForNakl(function (result)
            { kis_owner.list = result; });
        },
        getOwner: function (id) {
            var owner = getObjects(this.list, 'SOBSTV', id)
            if (owner != null && owner.length > 0) {
                return owner[0];
            }
        },
    },
    // список стран
    kis_strana = {
        list: [],
        initObject: function () {
            getAsyncKometaStrana(function (result)
            { kis_strana.list = result; });
        },
        getStrana: function (id) {
            var strana = getObjects(this.list, 'KOD_STRAN', id)
            if (strana != null && strana.length > 0) {
                return strana[0];
            }
        },
    },
    // Список Vagon
    Vagons = {
        list: [],
        addVagon: function (id, route, natur, day, month, year, hour, minute) {
            switch (route) {
                case 0:
                    getAsyncArrivalPromVagon(natur, day, month, year, hour, minute,
                    function (result) {
                        Vagons.list.push({ id: id, vagon: result });
                        table_turnover.addVagon(id, result.length);

                    });
                    break;
                case 1:
                    getAsyncSendingPromVagon(natur, day, month, year, hour, minute,
                    function (result) {
                        Vagons.list.push({ id: id, vagon: result });
                        table_turnover.addVagon(id, result.length);
                    });
                    break;
                default:
                    Vagons.list.push({ id: id, vagon: null });
                    break;
            };
        },
        getVagon: function (id) {
            var list_vagon = getObjects(Vagons.list, 'id', id)
            if (list_vagon != null && list_vagon.length > 0) {
                return list_vagon[0];
            }
        }
    }
    // Список Nat_Hist
    Nat_Hist = {
        list: [],
        addNatHist: function (id, route, natur, day, month, year, hour, minute) {
            switch (route) {
                case 0:
                    getAsyncArrivalPromNatHist(natur, day, month, year, hour, minute,
                    function (result) {
                        Nat_Hist.list.push({ id: id, vagon: result });
                        table_turnover.addNatHist(id, result.length);

                    });
                    break;
                case 1:
                    getAsyncSendingNPromatHist(natur, day, month, year, hour, minute,
                    function (result) {
                        Nat_Hist.list.push({ id: id, vagon: result });
                        table_turnover.addNatHist(id, result.length);
                    });
                    break;
                default:
                    Nat_Hist.list.push({ id: id, vagon: null });
                    break;
            };
        },
        getNatHist: function (id) {
            var list_vagon = getObjects(Nat_Hist.list, 'id', id)
            if (list_vagon != null && list_vagon.length > 0) {
                return list_vagon[0];
            }
        }
    }
    // Панель таблицы
    panel_table_turnover = {
        html_div_panel: $('<div class="dt-buttons setup-operation" id="property"></div>'),
        //html_div_panel_info: $('<div class="setup-operation" id="last-info"></div>'),
        html_div_panel_select: $('<div class="setup-operation" id="last-select"></div>'),

        label_last_total: $('<label class="label-text" for="label-last-total-value"></label>'),
        label_last_total_value: $('<label class="value-text" id="label-last-total-value"></label>'),

        button_close_detali: $('<button class="ui-button ui-widget ui-corner-all"></button>'),
        initPanel: function (obj) {
            // Настроим панель info
            //this.html_div_panel_info
            //.append(this.label_last_date)
            //.append(this.label_last_date_value)
            //.append(this.label_last_total.text(resurses.getText("label_total_cars")))
            //.append(this.label_last_total_value);

            this.html_div_panel_select
                .append(this.button_close_detali.text(resurses.getText("button_close_detali")))

            this.html_div_panel
                //.append(this.html_div_panel_info)
                .append(this.html_div_panel_select);
            obj.prepend(this.html_div_panel);
            // Обработка события закрыть детали
            this.button_close_detali.on('click', function () {
                var trs = $('tr.shown');

                for (i = 0; i < trs.length; i++) {
                    var row = table_turnover.obj.row(trs[i]);
                    if (row.child.isShown()) {
                        // This row is already open - close it
                        row.child.hide();
                    }
                }
                $('tr').removeClass('shown');
            });
        },
    },
    // Оборот вагонов
    table_turnover = {
        html_table: $('#table-list-turnover'),
        html_div_panel: $('<div class="dt-buttons setup-operation" id="property"></div>'),
        //button_to_excel_detali: $('<button class="ui-button ui-widget ui-corner-all" id=""></button>'),
        obj_table: null,
        obj: null,
        list: null,
        // Инициализация вагонов
        initObject: function () {
            this.obj = this.html_table.DataTable({
                //"lengthMenu": [10, 25, 50, 100, 200, 400],
                "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                "paging": true,
                "ordering": true,
                "info": false,
                "select": true,
                //"filter": true,
                //"scrollY": "600px",
                "scrollX": true,
                language: {
                    emptyTable: resurses.getText("table_message_emptyTable"),
                    decimal: resurses.getText("table_decimal"),
                    search: resurses.getText("table_message_search"),
                },
                jQueryUI: true,
                "createdRow": function (row, data, index) {
                    $(row).attr('id', data.ID).addClass(Operation(data.P_OT));
                },
                columns: [
                    {
                        className: 'details-control',
                        orderable: false,
                        data: null,
                        defaultContent: '',
                        searchable: false, width: "30px"
                    },
                    { data: "N_NATUR", title: resurses.getText("table_field_N_NATUR"), orderable: true, searchable: true },
                    { data: "DT_PR", title: resurses.getText("table_field_DT_PR"), width: "150px", orderable: true, searchable: false },
                    { data: "DT", title: resurses.getText("table_field_DT"), width: "150px", orderable: true, searchable: false },
                    { data: "CountVagon", title: resurses.getText("table_field_CountVagon"), orderable: true, searchable: false },
                    { data: "CountNatHist", title: resurses.getText("table_field_NatHist"), orderable: true, searchable: false },
                    //{ data: "D_PR_DD", title: resurses.getText("table_field_D_PR_DD"), orderable: true, searchable: true },
                    //{ data: "D_PR_MM", title: resurses.getText("table_field_D_PR_MM"), orderable: true, searchable: false },
                    //{ data: "D_PR_YY", title: resurses.getText("table_field_D_PR_YY"), orderable: true, searchable: false },
                    //{ data: "T_PR_HH", title: resurses.getText("table_field_T_PR_HH"), orderable: true, searchable: true },
                    //{ data: "T_PR_MI", title: resurses.getText("table_field_T_PR_MI"), orderable: true, searchable: false },
                    //{ data: "D_DD", title: resurses.getText("table_field_D_DD"), orderable: true, searchable: true },
                    //{ data: "D_MM", title: resurses.getText("table_field_D_MM"), orderable: true, searchable: false },
                    //{ data: "D_YY", title: resurses.getText("table_field_D_YY"), orderable: true, searchable: false },
                    //{ data: "T_HH", title: resurses.getText("table_field_T_HH"), orderable: true, searchable: true },
                    //{ data: "T_MI", title: resurses.getText("table_field_T_MI"), orderable: true, searchable: false },
                    //{ data: "K_ST", title: resurses.getText("table_field_K_ST"), orderable: true, searchable: false },
                    { data: "Station", title: resurses.getText("table_field_K_ST"), orderable: false, searchable: true },
                    { data: "N_PUT", title: resurses.getText("table_field_N_PUT"), orderable: true, searchable: false },
                    { data: "NAPR", title: resurses.getText("table_field_NAPR"), orderable: true, searchable: false },
                    //{ data: "P_OT", title: resurses.getText("table_field_P_OT"), orderable: true, searchable: false },
                    { data: "Operation", title: resurses.getText("table_field_P_OT"), orderable: false, searchable: true },
                    //{ data: "V_P", title: resurses.getText("table_field_V_P"), orderable: true, searchable: false },
                    { data: "CloseInput", title: resurses.getText("table_field_V_P"), orderable: true, searchable: false },
                    //{ data: "K_ST_OTPR", title: resurses.getText("table_field_K_ST_OTPR"), orderable: true, searchable: true },
                    { data: "Station_from", title: resurses.getText("table_field_K_ST_OTPR"), orderable: false, searchable: true },
                    //{ data: "K_ST_PR", title: resurses.getText("table_field_K_ST_PR"), orderable: true, searchable: false },
                    { data: "Station_on", title: resurses.getText("table_field_K_ST_PR"), orderable: false, searchable: true },
                    //{ data: "N_VED_PR", title: resurses.getText("table_field_N_VED_PR"), orderable: true, searchable: false },
                    //{ data: "N_SOST_OT", title: resurses.getText("table_field_N_SOST_OT"), orderable: true, searchable: false },
                    //{ data: "N_SOST_PR", title: resurses.getText("table_field_N_SOST_PR"), orderable: true, searchable: false },
                    { data: "DAT_VVOD", title: resurses.getText("table_field_DAT_VVOD"), width: "150px", orderable: true, searchable: false },
                ],

            });
            this.obj_table = $('DIV#table-list-turnover_wrapper');
            panel_table_turnover.initPanel(this.obj_table);
            this.initEventSelectChild();
        },
        // Показать таблицу
        viewTable: function (data_refresh) {
            OnBegin();
            if (this.list == null | data_refresh == true) {
                // Обновим данные
                getAsyncPromSostav(
                    start, stop,
                    function (result) {
                        table_turnover.list = result;
                        //panel_operations_last.label_last_total_value.text(result.length);
                        table_turnover.loadDataTable(result);
                        table_turnover.initComplete();
                        table_turnover.obj.draw();
                    }
                    );
            } else {
                table_turnover.loadDataTable(this.list);
                table_turnover.initComplete();
                table_turnover.obj.draw();
            };
        },
        // Загрузить данные в таблицу
        loadDataTable: function (data) {
            this.list = data;
            this.obj.clear();

            for (i = 0; i < data.length; i++) {
                var station = rw_stations.selectStationKIS(data[i].K_ST);
                var station_from = rw_stations.selectStationKIS(data[i].K_ST_OTPR);
                var station_on = rw_stations.selectStationKIS(data[i].K_ST_PR);
                //Vagons.addVagon(data[i].ID, data[i].P_OT, data[i].N_NATUR, data[i].D_DD, data[i].D_MM, data[i].D_YY, data[i].T_HH, data[i].T_MI);
                //Nat_Hist.addNatHist(data[i].ID, data[i].P_OT, data[i].N_NATUR, data[i].D_DD, data[i].D_MM, data[i].D_YY, data[i].T_HH, data[i].T_MI);
                this.obj.row.add({
                    "ID": data[i].ID,
                    "N_NATUR": data[i].N_NATUR,
                    "DT_PR": data[i].DT_PR,
                    "DT": data[i].DT,
                    "D_PR_DD": data[i].D_PR_DD,
                    "D_PR_MM": data[i].D_PR_MM,
                    "D_PR_YY": data[i].D_PR_YY,
                    "T_PR_HH": data[i].T_PR_HH,
                    "T_PR_MI": data[i].T_PR_MI,
                    "D_DD": data[i].D_DD,
                    "D_MM": data[i].D_MM,
                    "D_YY": data[i].D_YY,
                    "T_HH": data[i].T_HH,
                    "T_MI": data[i].T_MI,
                    "K_ST": data[i].K_ST,
                    "Station": data[i].K_ST != null ? station != null ? (lang == 'en' ? station.name_en : station.name_ru) : '?' : '-',
                    "N_PUT": data[i].N_PUT,
                    "NAPR": data[i].NAPR,
                    "P_OT": data[i].P_OT,
                    "Operation": resurses.getText(Operation(data[i].P_OT)),
                    "V_P": data[i].V_P,
                    "CloseInput": resurses.getText(CloseInput(data[i].V_P)),
                    "K_ST_OTPR": data[i].K_ST_OTPR,
                    "Station_from": data[i].K_ST_OTPR != null ? station_from != null ? (lang == 'en' ? station_from.name_en : station_from.name_ru) : '?' : '',
                    "K_ST_PR": data[i].K_ST_PR,
                    "Station_on": data[i].K_ST_PR != null ? station_on != null ? (lang == 'en' ? station_on.name_en : station_on.name_ru) : '?' : '',
                    "N_VED_PR": data[i].N_VED_PR,
                    "N_SOST_OT": data[i].N_SOST_OT,
                    "N_SOST_PR": data[i].N_SOST_PR,
                    "DAT_VVOD": data[i].DAT_VVOD,
                    "CountVagon": data[i].countVagon,
                    "CountNatHist": data[i].countNatHist,
                });
            }
            LockScreenOff();
        },
        // Показать выборку по полям таблицы
        initComplete: function () {
            table_turnover.obj.columns([6, 9, 11, 12]).every(function () {
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
        // Инициализация события выборки детально
        initEventSelectChild: function () {
            this.html_table.find('tbody')
            .on('click', 'td.details-control', function () {
                var tr = $(this).closest('tr');
                var row = table_turnover.obj.row(tr);
                if (row.child.isShown()) {
                    // This row is already open - close it
                    row.child.hide();
                    tr.removeClass('shown');
                }
                else {
                    row.child('<div id="detali-turnover' + row.data().ID + '" class="detali-operation"> ' +
                        '<div id="tabs-detali' + row.data().ID + '"> ' +
                            '<ul> ' +
                                '<li><a href="#tabs-detali' + row.data().ID + '-1" id="link-tabs-detali' + row.data().ID + '-1"></a></li> ' +
                                '<li><a href="#tabs-detali' + row.data().ID + '-2" id="link-tabs-detali' + row.data().ID + '-2"></a></li> ' +
                                '<li><a href="#tabs-detali' + row.data().ID + '-3" id="link-tabs-detali' + row.data().ID + '-3"></a></li> ' +
                            '</ul> ' +
                            '<div id="tabs-detali' + row.data().ID + '-1"> ' +

                            '</div> ' +
                            '<div id="tabs-detali' + row.data().ID + '-2"> ' +

                            '</div> ' +
                            '<div id="tabs-detali' + row.data().ID + '-3" class="tabs-detali"> ' +

                            '</div> ' +
                        '</div> ' +
                        '</div>').show();
                    // Инициализируем
                    $('#link-tabs-detali' + row.data().ID + '-1').text(resurses.getText("link_tabs_turnover_detali_1"));
                    $('#link-tabs-detali' + row.data().ID + '-2').text(resurses.getText("link_tabs_turnover_detali_2"));
                    $('#link-tabs-detali' + row.data().ID + '-3').text(resurses.getText("link_tabs_turnover_detali_3"));
                    $('#tabs-detali' + row.data().ID).tabs({
                        collapsible: true,
                    });
                    //table_turnover.viewTableChildAllFields(row.data());
                    table_turnover.viewTableChildVagon(row.data());
                    table_turnover.viewTableChildNatHist(row.data());
                    tr.addClass('shown');
                }
            });
        },
        // Показать таблицу Prom.Vagon
        viewTableChildVagon: function (data) {
            var list_vagon = Vagons.getVagon(data.ID);
            var target = $("#tabs-detali" + data.ID + "-2");
            target.empty();
            var tab = this.createTableVagon(list_vagon);
            target.append(tab);
        },
        // Создать таблицу Prom.Vagon
        createTableVagon: function (data) {
            if (data == null || data.length == 0) {
                return resurses.getText("table_not_data")
            }

            var list_tr = '<thead><tr>' +
                '<th>' + resurses.getText("table_field_num_car") + '</th>' +
                '<th>' + resurses.getText("table_field_position") + '</th>' +
                '<th>' + resurses.getText("table_field_DT_PR") + '</th>' +
                '<th>' + resurses.getText("table_field_DT_SD") + '</th>' +
                '<th>' + resurses.getText("table_field_GODN") + '</th>' +
            '</tr></thead>';
            list_tr += '<tbody>';
            var vagons = data.vagon;
            for (i = 0; i < vagons.length; i++) {
                //var rod_cargo = lang == 'en' ? data[i].type_cargo_en : data[i].type_cargo_ru;
                //var st_dislocation = data[i].st_disl != null ? data[i].nst_disl + '(' + data[i].st_disl + ')' : '';
                //var st_naznach = data[i].st_nazn != null ? data[i].nst_nazn + '(' + data[i].st_nazn + ')' : '';
                //var st_form = data[i].st_form != null ? data[i].nst_form + '(' + data[i].st_form + ')' : '';
                //var st_end = data[i].st_end != null ? data[i].nst_end + '(' + data[i].st_end + ')' : '';
                list_tr += '<tr>' +
                    '<td>' + vagons[i].N_VAG + '</td>' +
                    '<td>' + (vagons[i].NPP != null ? vagons[i].NPP : '') + '</td>' +
                    '<td>' + (vagons[i].DT_PR != null ? vagons[i].DT_PR : '') + '</td>' +
                    '<td>' + (vagons[i].DT_SD != null ? vagons[i].DT_SD : '') + '</td>' +
                    '<td>' + vagons[i].GODN + '</td>' +
                    //'<td>' + data[i].dt + '</td>' +
                    //'<td>' + st_dislocation + '</td>' +
                    //'<td>' + st_naznach + '</td>' +
                    //'<td>' + data[i].index + '</td>' +
                    //'<td>' + st_form + '</td>' +
                    //'<td>' + outVal(data[i].kgro) + '</td>' +
                    //'<td>' + st_end + '</td>' +
                    //'<td>' + outVal(data[i].kgrp) + '</td>' +
                    //'<td>' + outVal(data[i].km) + '</td>' +
                    //'<td>' + data[i].ves + '</td>' +
                    //'<td>' + rod_cargo + '(' + data[i].kgr + ')' + '</td>' +
                    '</tr>';
            }
            list_tr += '</tbody>';
            return '<table class="table-turnover-detali" id="table-detali-vagon' + data.id + '" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' + list_tr + '</table>';
        },
        // Показать таблицу Prom.Nat_Hist
        viewTableChildNatHist: function (data) {
            var list_vagon = Nat_Hist.getNatHist(data.ID);
            var target = $("#tabs-detali" + data.ID + "-3");
            target.empty();
            var tab = this.createTableNatHist(list_vagon);
            target.append(tab);
            $('#table-detali-nathist' + data.ID).DataTable({
                "paging": false,
                "ordering": false,
                "info": false,
                "select": false,
                //"filter": true,
                //"scrollY": "600px",
                "scrollX": true,
                language: {
                    emptyTable: resurses.getText("table_message_emptyTable"),
                    decimal: resurses.getText("table_decimal"),
                    search: resurses.getText("table_message_search"),
                },
                jQueryUI: true,
                columns: [
                        { data: "N_VAG", title: resurses.getText("table_field_num_car") },
                        { data: "NPP", title: resurses.getText("table_field_position") },
                        { data: "DT_PR", title: resurses.getText("table_field_DT_PR") },
                        { data: "DT_SD", title: resurses.getText("table_field_DT_SD") },
                        { data: "GODN", title: resurses.getText("table_field_GODN") },
                        { data: "K_POL_GR", title: resurses.getText("table_field_K_POL_GR") },
                        { data: "K_GR", title: resurses.getText("table_field_K_GR") },
                        { data: "N_VED_PR", title: resurses.getText("table_field_N_VED_PR") },
                        { data: "N_NAK_MPS", title: resurses.getText("table_field_N_NAK_MPS") },
                        { data: "OTPRAV", title: resurses.getText("table_field_OTPRAV") },
                        { data: "PRIM_GR", title: resurses.getText("table_field_PRIM_GR") },
                        { data: "WES_GR", title: resurses.getText("table_field_WES_GR") },
                        //{ data: "N_NATUR", title: resurses.getText("table_field_N_NATUR") },
                        //{ data: "K_ST", title: resurses.getText("table_field_K_ST") },
                        //{ data: "N_PUT", title: resurses.getText("table_field_N_PUT") },
                        { data: "K_OP", title: resurses.getText("table_field_K_OP") },
                        { data: "K_FRONT", title: resurses.getText("table_field_K_FRONT") },
                        //{ data: "N_NATUR_T", title: resurses.getText("table_field_N_NATUR_T") },
                        { data: "GODN_T", title: resurses.getText("table_field_GODN_T") },
                        { data: "K_GR_T", title: resurses.getText("table_field_K_GR_T") },
                        { data: "WES_GR_T", title: resurses.getText("table_field_WES_GR_T") },
                        { data: "K_OTPR_GR", title: resurses.getText("table_field_K_OTPR_GR") },
                        //{ data: "K_ST_OTPR", title: resurses.getText("table_field_K_ST_OTPR") },
                        //{ data: "K_ST_NAZN", title: resurses.getText("table_field_K_ST_NAZN") },
                        { data: "ST_OTPR", title: resurses.getText("table_field_ST_OTPR") },
                        { data: "ZADER", title: resurses.getText("table_field_ZADER") },
                        { data: "NEPR", title: resurses.getText("table_field_NEPR") },
                        { data: "UDOST", title: resurses.getText("table_field_UDOST") },
                        { data: "SERTIF", title: resurses.getText("table_field_SERTIF") },
                        { data: "KOD_STRAN", title: resurses.getText("table_field_KOD_STRAN") },
                        { data: "KOD_SD", title: resurses.getText("table_field_KOD_SD") },
                        { data: "NETO", title: resurses.getText("table_field_NETO") },
                        { data: "BRUTO", title: resurses.getText("table_field_BRUTO") },
                        { data: "TARA", title: resurses.getText("table_field_TARA") },
                        { data: "DAT_VVOD", title: resurses.getText("table_field_DAT_VVOD") },
                ],
            });
        },
        // Создать таблицу Prom.Nat_Hist
        createTableNatHist: function (data) {
            if (data == null || data.length == 0) {
                return resurses.getText("table_not_data")
            }

            var list_tr = '<thead><tr>' +
                '<th>' + resurses.getText("table_field_num_car") + '</th>' +
                '<th>' + resurses.getText("table_field_position") + '</th>' +
                '<th>' + resurses.getText("table_field_DT_PR") + '</th>' +
                '<th>' + resurses.getText("table_field_DT_SD") + '</th>' +
                '<th>' + resurses.getText("table_field_GODN") + '</th>' +
                '<th>' + resurses.getText("table_field_K_POL_GR") + '</th>' +
                '<th>' + resurses.getText("table_field_K_GR") + '</th>' +
                '<th>' + resurses.getText("table_field_N_VED_PR") + '</th>' +
                '<th>' + resurses.getText("table_field_N_NAK_MPS") + '</th>' +
                '<th>' + resurses.getText("table_field_OTPRAV") + '</th>' +
                '<th>' + resurses.getText("table_field_PRIM_GR") + '</th>' +
                '<th>' + resurses.getText("table_field_WES_GR") + '</th>' +
                //'<th>' + resurses.getText("table_field_N_NATUR") + '</th>' +
                //'<th>' + resurses.getText("table_field_K_ST") + '</th>' +
                //'<th>' + resurses.getText("table_field_N_PUT") + '</th>' +
                '<th>' + resurses.getText("table_field_K_OP") + '</th>' +
                '<th>' + resurses.getText("table_field_K_FRONT") + '</th>' +
                //'<th>' + resurses.getText("table_field_N_NATUR_T") + '</th>' +
                '<th>' + resurses.getText("table_field_GODN_T") + '</th>' +
                '<th>' + resurses.getText("table_field_K_GR_T") + '</th>' +
                '<th>' + resurses.getText("table_field_WES_GR_T") + '</th>' +
                '<th>' + resurses.getText("table_field_K_OTPR_GR") + '</th>' +
                //'<th>' + resurses.getText("table_field_K_ST_OTPR") + '</th>' +
                //'<th>' + resurses.getText("table_field_K_ST_NAZN") + '</th>' +
                '<th>' + resurses.getText("table_field_ST_OTPR") + '</th>' +
                '<th>' + resurses.getText("table_field_ZADER") + '</th>' +
                '<th>' + resurses.getText("table_field_NEPR") + '</th>' +
                '<th>' + resurses.getText("table_field_UDOST") + '</th>' +
                '<th>' + resurses.getText("table_field_SERTIF") + '</th>' +
                '<th>' + resurses.getText("table_field_KOD_STRAN") + '</th>' +
                '<th>' + resurses.getText("table_field_KOD_SD") + '</th>' +
                '<th>' + resurses.getText("table_field_NETO") + '</th>' +
                '<th>' + resurses.getText("table_field_BRUTO") + '</th>' +
                '<th>' + resurses.getText("table_field_TARA") + '</th>' +
                '<th>' + resurses.getText("table_field_DAT_VVOD") + '</th>' +
            '</tr></thead>';
            list_tr += '<tbody>';
            var vagons = data.vagon;
            for (i = 0; i < vagons.length; i++) {
                //var rod_cargo = lang == 'en' ? data[i].type_cargo_en : data[i].type_cargo_ru;
                //var st_dislocation = data[i].st_disl != null ? data[i].nst_disl + '(' + data[i].st_disl + ')' : '';
                //var st_naznach = data[i].st_nazn != null ? data[i].nst_nazn + '(' + data[i].st_nazn + ')' : '';
                //var st_form = data[i].st_form != null ? data[i].nst_form + '(' + data[i].st_form + ')' : '';
                //var st_end = data[i].st_end != null ? data[i].nst_end + '(' + data[i].st_end + ')' : '';
                var condition = rw_car_condition.getСondition(vagons[i].GODN);
                var condition_t = rw_car_condition.getСondition(vagons[i].GODN_T);
                var cex = kis_cex.getCex(vagons[i].K_POL_GR);
                var gruz = kis_gruz.getGruz(vagons[i].K_GR);
                var gruz_t = kis_gruz.getGruz(vagons[i].K_GR_T);
                var station_op = rw_stations.selectStationKIS(vagons[i].K_ST);
                var route = resurses.getText(Route(vagons[i].K_OP))
                var owner = kis_owner.getOwner(vagons[i].K_FRONT);
                var station_from = rw_stations.selectStationKIS(vagons[i].K_ST_OTPR);
                var station_on = rw_stations.selectStationKIS(vagons[i].K_ST_NAZN);
                var strana = kis_strana.getStrana(vagons[i].KOD_STRAN);
                list_tr += '<tr>' +
                    '<td>' + vagons[i].N_VAG + '</td>' +
                    '<td>' + (vagons[i].NPP != null ? vagons[i].NPP : '') + '</td>' +
                    '<td>' + (vagons[i].DT_PR != null ? vagons[i].DT_PR : '') + '</td>' +
                    '<td>' + (vagons[i].DT_SD != null ? vagons[i].DT_SD : '') + '</td>' +
                    '<td>' + (vagons[i].GODN != null ? '(' + vagons[i].GODN + ') - ' + (condition != null ? (lang == 'en' ? condition.name_en : condition.name_ru) : '?') : '') + '</td>' +  // rw_car_condition
                    '<td>' + (vagons[i].K_POL_GR != null ? '(' + vagons[i].K_POL_GR + ') - ' + (cex != null ? cex.NAME_P : '') : '') + '</td>' +  // kis_cex
                    '<td>' + (vagons[i].K_GR != null ? '(' + vagons[i].K_GR + ') - ' + (gruz != null ? gruz.NAME_GR + '(' + gruz.TAR_GR + ')' : '') : '') + '</td>' +  // kis_gruz
                    '<td>' + (vagons[i].N_VED_PR != null ? vagons[i].N_VED_PR : '') + '</td>' +
                    '<td>' + (vagons[i].N_NAK_MPS != null ? vagons[i].N_NAK_MPS : '') + '</td>' +
                    '<td>' + (vagons[i].OTPRAV != null ? vagons[i].OTPRAV : '') + '</td>' +
                    '<td>' + (vagons[i].PRIM_GR != null ? vagons[i].PRIM_GR : '') + '</td>' +
                    '<td>' + (vagons[i].WES_GR != null ? vagons[i].WES_GR : '') + '</td>' +
                    //'<td>' + (vagons[i].N_NATUR != null ? vagons[i].N_NATUR : '') + '</td>' +
                    //'<td>' + (vagons[i].K_ST != null ? '(' + vagons[i].K_ST + ') - ' + (station_op != null ? (lang == 'en' ? station_op.name_en : station_op.name_ru) : '?') : '') + '</td>' +  // rw_stations
                    //'<td>' + (vagons[i].N_PUT != null ? vagons[i].N_PUT : '') + '</td>' +
                    '<td>' + (vagons[i].K_OP != null ? '(' + vagons[i].K_OP + ') - ' + (route != null ? route : '') : '') + '</td>' +  // Route
                    '<td>' + (vagons[i].K_FRONT != null ? '(' + vagons[i].K_FRONT + ') - ' + (owner != null ? owner.NPLAT : '') : '') + '</td>' +  // kis_owner
                    //'<td>' + (vagons[i].N_NATUR_T != null ? vagons[i].N_NATUR_T : '') + '</td>' +
                    '<td>' + (vagons[i].GODN_T != null ? '(' + vagons[i].GODN_T + ') - ' + (condition_t != null ? (lang == 'en' ? condition_t.name_en : condition_t.name_ru) : '?') : '') + '</td>' +  // rw_car_condition
                    '<td>' + (vagons[i].K_GR_T != null ? '(' + vagons[i].K_GR_T + ') - ' + (gruz_t != null ? gruz_t.NAME_GR + '(' + gruz_t.TAR_GR + ')' : '') : '') + '</td>' +  // kis_gruz
                    '<td>' + (vagons[i].WES_GR_T != null ? vagons[i].WES_GR_T : '') + '</td>' +
                    '<td>' + (vagons[i].K_OTPR_GR != null ? vagons[i].K_OTPR_GR : '') + '</td>' +
                    //'<td>' + (vagons[i].K_ST_OTPR != null ? '(' + vagons[i].K_ST_OTPR + ') - ' + (station_from != null ? (lang == 'en' ? station_from.name_en : station_from.name_ru) : '?') : '') + '</td>' +  // rw_stations
                    //'<td>' + (vagons[i].K_ST_NAZN != null ? '(' + vagons[i].K_ST_NAZN + ') - ' + (station_on != null ? (lang == 'en' ? station_on.name_en : station_on.name_ru) : '?') : '') + '</td>' +  // rw_stations
                    '<td>' + (vagons[i].ST_OTPR != null ? vagons[i].ST_OTPR : '') + '</td>' +
                    '<td>' + (vagons[i].ZADER != null ? vagons[i].ZADER : '') + '</td>' +
                    '<td>' + (vagons[i].NEPR != null ? vagons[i].NEPR : '') + '</td>' +
                    '<td>' + (vagons[i].UDOST != null ? vagons[i].UDOST : '') + '</td>' +
                    '<td>' + (vagons[i].SERTIF != null ? vagons[i].SERTIF : '') + '</td>' +
                    '<td>' + (vagons[i].KOD_STRAN != null ? '(' + vagons[i].KOD_STRAN + ') - ' + (strana != null ? strana.NAME : '') : '') + '</td>' +  // kis_strana
                    '<td>' + (vagons[i].KOD_SD != null ? vagons[i].KOD_SD : '') + '</td>' +
                    '<td>' + (vagons[i].NETO != null ? vagons[i].NETO : '') + '</td>' +
                    '<td>' + (vagons[i].BRUTO != null ? vagons[i].BRUTO : '') + '</td>' +
                    '<td>' + (vagons[i].TARA != null ? vagons[i].TARA : '') + '</td>' +
                    '<td>' + (vagons[i].DAT_VVOD != null ? vagons[i].DAT_VVOD : '') + '</td>' +
                    '</tr>';
            }
            list_tr += '</tbody>';
            return '<table class="compact" id="table-detali-nathist' + data.id + '" cellpadding="5" cellspacing="0" border="0" style="width:100%">' + list_tr + '</table>';
        },


        viewTableChild: function (data) {
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
        },
        // Добавить значения количества вагонов по таблице Prom.Vagon
        addVagon: function (id, length) {
            var row = table_turnover.obj.rows('#' + id).nodes();
            row[0].cells(4).innerText = length;
        },
        // Добавить значения количества вагонов по таблице Prom.Vagon
        addNatHist: function (id, length) {
            var row = table_turnover.obj.rows('#' + id).nodes();
            row[0].cells(5).innerText = length;
        }

    }
    //-----------------------------------------------------------------------------------------
    // Функции
    //-----------------------------------------------------------------------------------------
    var outVal = function (i) {
        return i != null ? Number(i) : '';
    };
    //Вернуть вид операции
    var Operation = function (i) {
        switch (i) {
            case 0:
                return 'operation_input';
            case 1:
                return 'operation_output';
            default:
                return 'operation_null';
        }
    };
    // Вернуть признак конца ввода прибытия
    var CloseInput = function (i) {
        switch (i) {
            case 1:
                return 'close_input';
            default:
                return 'close_null';
        }
    };
    //Вернуть вид операции
    var Route = function (i) {
        switch (i) {
            case 0:
                return 'route_not';
            case 1:
                return 'route_ok';
            default:
                return 'route_null';
        }
    };

    //-----------------------------------------------------------------------------------------
    // Инициализация объектов
    //-----------------------------------------------------------------------------------------
    resurses.initObject("/railway/Scripts/KIS/kis.json",
    function () {
        // Загружаем дальше
        $('#label-select-date-range').text(resurses.getText("label-select-date-range"));
        $('#label-to').text(resurses.getText("table_message_separator"));
        $('#to-excel').text(resurses.getText("button_to_excel"));
        datetime_range.initObject();
        rw_stations.initObject();       // станции
        rw_car_condition.initObject();  // годности
        kis_cex.initObject();           // цеха
        kis_gruz.initObject();          // грузы
        kis_owner.initObject();         // собственики
        kis_strana.initObject();         // страны

        table_turnover.initObject();
        tab_type_turnover.initObject(); // Типы закладок отчетов 

    }); // локализация



});
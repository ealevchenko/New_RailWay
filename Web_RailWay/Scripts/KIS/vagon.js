$(function () {
    //-----------------------------------------------------------------------------------------
    // Объявление глобальных переменных
    //-----------------------------------------------------------------------------------------
    allVars = $.getUrlVars();   // Получить параметры get запроса
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
    svagon = $("#vagon").spinner({
        spin: function (event, ui) {
            if (ui.value > 99999999) {
                $(this).spinner("value", 1);
                return false;
            } else if (ui.value < 1) {
                $(this).spinner("value", 99999999);
                return false;
            }
        }
    }),
    bt_searsh = $('#search').button({
        icon: "ui-icon-search"
    }).on('click', function (evt) {
        evt.preventDefault();
        table_vagon.viewTables(true);
    }),
    accordion_type = {
        html_div1: $("#accordion-vagon"),
        html_div2: $("#accordion-nathist"),
        icons: {
            header: "ui-icon-circle-arrow-e",
            activeHeader: "ui-icon-circle-arrow-s"
        },
        initObject: function () {
            this.html_div1.accordion({
                icons: this.icons,
                heightStyle: "content",
                collapsible: true,
                activate: function (event, ui) {
                    var active = accordion_type.html_div1.accordion("option", "active");
                    if (active === 0) {
                        table_vagon.viewTableVagon(false);
                    }
                }
            });
            this.html_div2.accordion({
                icons: this.icons,
                heightStyle: "content",
                collapsible: true,
                activate: function (event, ui) {
                    var active = accordion_type.html_div2.accordion("option", "active");
                    if (active === 0) {
                        table_vagon.viewTableNatHist(false);
                    }
                }
            });
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
    // ТАБЛИЦА VAGON
    table_vagon = {
        html_table_vagon: $('#table-list-vagon'),
        html_table_nathist: $('#table-list-nathist'),
        //html_div_panel: $('<div class="dt-buttons setup-operation" id="property"></div>'),
        //button_to_excel_detali: $('<button class="ui-button ui-widget ui-corner-all" id=""></button>'),
        obj_table_vagon: $('DIV#table-list-vagon_wrapper'),
        obj_table_nathist: $('DIV#table-list-vagon_wrapper'),
        obj_vagon: null,
        obj_nathist: null,
        list_vagon: null,
        list_nathist: null,
        // Инициализация вагонов
        initObject: function () {
            this.obj_vagon = this.initObjectTables(this.html_table_vagon);
            this.obj_nathist = this.initObjectTables(this.html_table_nathist);
        },
        // Инициализация каждой таблицы
        initObjectTables: function (html_table) {
            return html_table.DataTable({
                "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                "paging": true,
                "ordering": true,
                "info": false,
                "select": false,
                "autoWidth": false,
                //"filter": true,
                //"scrollY": "400px",
                "scrollX": true,
                language: {
                    emptyTable: resurses.getText("table_message_emptyTable"),
                    decimal: resurses.getText("table_decimal"),
                    search: resurses.getText("table_message_search"),
                },
                jQueryUI: true,
                "createdRow": function (row, data, index) {
                    $(row).attr('id', data.ID).addClass(Operation(data.P_OT));
                    var link_kis = $('<a id=' + data.id + ' target="_blank" href="/railway/KIS/Home/Natur?natur=' + data.N_NATUR +
                        '&day=' + data.D_DD +
                        '&month=' + data.D_MM +
                        '&year=' + data.D_YY +
                        '&hour=' + data.T_HH +
                        '&minute=' + data.T_MI +
                        '">' + data.N_NATUR + '</a>');
                    $('td', row).eq(3).html(link_kis);
                },
                columns: [
                        { data: "N_VAG", title: resurses.getText("table_field_num_car"), orderable: false, searchable: false },
                        { data: "NPP", title: resurses.getText("table_field_position"), orderable: false, searchable: false },
                        { data: "DT", title: resurses.getText("table_field_DT_PR_SD"), orderable: true, searchable: false }, //2
                        { data: "N_NATUR", title: resurses.getText("table_field_N_NATUR"), orderable: false, searchable: true },
                        { data: "N_NATUR_T", title: resurses.getText("table_field_N_NATUR_T"), orderable: false, searchable: true },
                        { data: "GODN", title: resurses.getText("table_field_GODN"), orderable: false, searchable: false },
                        { data: "GODN_T", title: resurses.getText("table_field_GODN_T"), orderable: false, searchable: false },
                        { data: "K_FRONT", title: resurses.getText("table_field_K_FRONT"), orderable: false, searchable: false },
                        { data: "KOD_STRAN", title: resurses.getText("table_field_KOD_STRAN"), orderable: false, searchable: false },

                        // Прибытие
                        { data: "DT_PR", title: resurses.getText("table_field_DT_PR"), orderable: true, searchable: false }, //2
                        { data: "DT_SD", title: resurses.getText("table_field_DT_SD"), orderable: true, searchable: false }, // 12

                        { data: "K_POL_GR", title: resurses.getText("table_field_K_POL_GR"), orderable: false, searchable: false },
                        { data: "K_GR", title: resurses.getText("table_field_K_GR"), orderable: false, searchable: false },
                        { data: "ST_OTPR", title: resurses.getText("table_field_ST_OTPR"), orderable: false, searchable: false },
                        { data: "OTPRAV", title: resurses.getText("table_field_OTPRAV"), orderable: false, searchable: false },
                        { data: "PRIM_GR", title: resurses.getText("table_field_PRIM_GR"), orderable: false, searchable: false },
                        { data: "WES_GR", title: resurses.getText("table_field_WES_GR"), orderable: false, searchable: false },
                        { data: "N_VED_PR", title: resurses.getText("table_field_N_VED_PR"), orderable: false, searchable: false },
                        { data: "N_NAK_MPS", title: resurses.getText("table_field_N_NAK_MPS"), orderable: false, searchable: false },

                        // Отправка
                        { data: "K_GR_T", title: resurses.getText("table_field_K_GR_T"), orderable: false, searchable: false },
                        { data: "WES_GR_T", title: resurses.getText("table_field_WES_GR_T"), orderable: false, searchable: false },
                        { data: "K_OTPR_GR", title: resurses.getText("table_field_K_OTPR_GR"), orderable: false, searchable: false },
                        { data: "K_OP", title: resurses.getText("table_field_K_OP"), orderable: false, searchable: false }, //17
                        // Общая

                        { data: "SERTIF", title: resurses.getText("table_field_SERTIF"), orderable: false, searchable: false }, //20
                        { data: "KOD_SD", title: resurses.getText("table_field_KOD_SD"), orderable: false, searchable: false }, //21


                        { data: "K_ST", title: resurses.getText("table_field_K_ST"), orderable: true, searchable: true },
                        { data: "N_PUT", title: resurses.getText("table_field_N_PUT"), orderable: false, searchable: false },

                        { data: "K_ST_OTPR", title: resurses.getText("table_field_K_ST_OTPR"), orderable: true, searchable: true },
                        { data: "K_ST_NAZN", title: resurses.getText("table_field_K_ST_NAZN"), orderable: true, searchable: true },

                        { data: "ZADER", title: resurses.getText("table_field_ZADER"), orderable: false, searchable: false },
                        { data: "NEPR", title: resurses.getText("table_field_NEPR"), orderable: false, searchable: false },
                        { data: "UDOST", title: resurses.getText("table_field_UDOST"), orderable: false, searchable: false },
                        { data: "NETO", title: resurses.getText("table_field_NETO"), orderable: false, searchable: false },
                        { data: "BRUTO", title: resurses.getText("table_field_BRUTO"), orderable: false, searchable: false },
                        { data: "TARA", title: resurses.getText("table_field_TARA"), orderable: false, searchable: false },
                        { data: "DAT_VVOD", title: resurses.getText("table_field_DAT_VVOD"), orderable: true, searchable: false },
                ],

            });
        },
        // Показать все таблицы
        viewTables: function (data_refresh) {
            this.viewTableVagon(data_refresh);
            this.viewTableNatHist(data_refresh);
        },
        // Показать таблицу Vagon
        viewTableVagon: function (data_refresh) {
            OnBegin();
            if (this.list_vagon == null | data_refresh == true) {
                // Обновим данные
                getAsyncPromVagonAndSostav(
                    svagon.spinner("value"),
                    function (result) {
                        table_vagon.list_vagon = result;
                        table_vagon.loadDataTable(table_vagon.obj_vagon, result);
                        table_vagon.obj_vagon.draw();
                    }
                    );
            } else {
                this.loadDataTable(this.obj_vagon, this.list_vagon);
                this.obj_vagon.draw();
            };
        },
        // Показать таблицу Vagon
        viewTableNatHist: function (data_refresh) {
            OnBegin();
            if (this.list_vagon == null | data_refresh == true) {
                // Обновим данные
                getAsyncPromNatHistAndSostav(
                    svagon.spinner("value"),
                    function (result) {
                        table_vagon.list_nathist = result;
                        table_vagon.loadDataTable(table_vagon.obj_nathist, result);
                        table_vagon.obj_nathist.draw();
                    }
                    );
            } else {
                this.loadDataTable(this.obj_nathist, this.list_nathist);
                this.obj_nathist.draw();
            };
        },
        // Загрузить данные в таблицу
        loadDataTable: function (obj, data) {
            if (obj != null) {
                obj.clear();
                for (i = 0; i < data.length; i++) {
                    var condition = rw_car_condition.getСondition(data[i].GODN);
                    var condition_t = rw_car_condition.getСondition(data[i].GODN_T);
                    var cex = kis_cex.getCex(data[i].K_POL_GR);
                    var gruz = kis_gruz.getGruz(data[i].K_GR);
                    var gruz_t = kis_gruz.getGruz(data[i].K_GR_T);
                    var station_op = rw_stations.selectStationKIS(data[i].K_ST);
                    var route = resurses.getText(Route(data[i].K_OP))
                    var owner = kis_owner.getOwner(data[i].K_FRONT);
                    var station_from = rw_stations.selectStationKIS(data[i].K_ST_OTPR);
                    var station_on = rw_stations.selectStationKIS(data[i].K_ST_NAZN);
                    var strana = kis_strana.getStrana(data[i].KOD_STRAN);
                    obj.row.add({
                        "N_VAG": data[i].N_VAG,
                        "NPP": data[i].NPP,
                        "DT_PR": data[i].DT_PR != null ? data[i].DT_PR : '',
                        "DT_SD": data[i].DT_SD != null ? data[i].DT_SD : '',
                        "GODN": data[i].GODN != null ? '(' + data[i].GODN + ') - ' + (condition != null ? (lang == 'en' ? condition.name_en : condition.name_ru) : '?') : '',
                        "K_POL_GR": data[i].K_POL_GR != null ? '(' + data[i].K_POL_GR + ') - ' + (cex != null ? cex.NAME_P : '') : '',// kis_cex
                        "K_GR": data[i].K_GR != null ? '(' + data[i].K_GR + ') - ' + (gruz != null ? gruz.NAME_GR + '(' + gruz.TAR_GR + ')' : '') : '',// kis_gruz
                        "OTPRAV": data[i].OTPRAV != null ? data[i].OTPRAV : '',
                        "PRIM_GR": data[i].PRIM_GR != null ? data[i].PRIM_GR : '',
                        "WES_GR": data[i].WES_GR != null ? data[i].WES_GR : '',
                        "K_FRONT": data[i].K_FRONT != null ? '(' + data[i].K_FRONT + ') - ' + (owner != null ? owner.NPLAT : '') : '',// kis_owner
                        "KOD_STRAN": data[i].KOD_STRAN != null ? '(' + data[i].KOD_STRAN + ') - ' + (strana != null ? strana.NAME : '') : '', // kis_strana
                        "N_VED_PR": data[i].N_VED_PR != null ? data[i].N_VED_PR : '',
                        "N_NAK_MPS": data[i].N_NAK_MPS != null ? data[i].N_NAK_MPS : '',
                        "N_NATUR": data[i].N_NATUR != null ? data[i].N_NATUR : '',
                        "K_ST": data[i].K_ST != null ? '(' + data[i].K_ST + ') - ' + (station_op != null ? (lang == 'en' ? station_op.name_en : station_op.name_ru) : '?') : '',
                        "N_PUT": data[i].N_PUT != null ? data[i].N_PUT : '',
                        "K_OP": data[i].K_OP != null ? '(' + data[i].K_OP + ') - ' + (route != null ? route : '') : '',// Route
                        "N_NATUR_T": data[i].N_NATUR_T != null ? data[i].N_NATUR_T : '',
                        "GODN_T": data[i].GODN_T != null ? '(' + data[i].GODN_T + ') - ' + (condition_t != null ? (lang == 'en' ? condition_t.name_en : condition_t.name_ru) : '?') : '',// rw_car_condition
                        "K_GR_T": data[i].K_GR_T != null ? '(' + data[i].K_GR_T + ') - ' + (gruz_t != null ? gruz_t.NAME_GR + '(' + gruz_t.TAR_GR + ')' : '') : '',// kis_gruz
                        "WES_GR_T": data[i].WES_GR_T != null ? data[i].WES_GR_T : '',
                        "K_OTPR_GR": data[i].K_OTPR_GR != null ? data[i].K_OTPR_GR : '',
                        "K_ST_OTPR": data[i].K_ST_OTPR != null ? '(' + data[i].K_ST_OTPR + ') - ' + (station_from != null ? (lang == 'en' ? station_from.name_en : station_from.name_ru) : '?') : '', // rw_stations
                        "K_ST_NAZN": data[i].K_ST_NAZN != null ? '(' + data[i].K_ST_NAZN + ') - ' + (station_on != null ? (lang == 'en' ? station_on.name_en : station_on.name_ru) : '?') : '', // rw_stations
                        "ST_OTPR": data[i].ST_OTPR != null ? data[i].ST_OTPR : '',
                        "ZADER": data[i].ZADER != null ? data[i].ZADER : '',
                        "NEPR": data[i].NEPR != null ? data[i].NEPR : '',
                        "UDOST": data[i].UDOST != null ? data[i].UDOST : '',
                        "SERTIF": data[i].SERTIF != null ? data[i].SERTIF : '',
                        "KOD_SD": data[i].KOD_SD != null ? data[i].KOD_SD : '',
                        "NETO": data[i].NETO != null ? data[i].NETO : '',
                        "BRUTO": data[i].BRUTO != null ? data[i].BRUTO : '',
                        "TARA": data[i].TARA != null ? data[i].TARA : '',
                        "DAT_VVOD": data[i].DAT_VVOD != null ? data[i].DAT_VVOD : '',
                        "P_OT": data[i].P_OT,
                        "DT": data[i].DT,
                        "D_DD": data[i].D_DD,
                        "D_MM": data[i].D_MM,
                        "D_YY": data[i].D_YY,
                        "T_HH": data[i].T_HH,
                        "T_MI": data[i].T_MI,
                    });
                }
            }
            LockScreenOff();
        },
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
    //// Вернуть признак конца ввода прибытия
    //var CloseInput = function (i) {
    //    switch (i) {
    //        case 1:
    //            return 'close_input';
    //        default:
    //            return 'close_null';
    //    }
    //};
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
        $('#label-select-vagon').text(resurses.getText("label_select_vagon"));

        //bt_searsh.text(resurses.getText("button_to_searsh"));
        //$('#searsh').text(resurses.getText("button_to_searsh"));
        //$('#to-excel').text(resurses.getText("button_to_excel"));


        //datetime_range.initObject();
        rw_stations.initObject();       // станции
        rw_car_condition.initObject();  // годности
        kis_cex.initObject();           // цеха
        kis_gruz.initObject();          // грузы
        kis_owner.initObject();         // собственики
        kis_strana.initObject();         // страны

        table_vagon.initObject();
        //tab_type_vagon.initObject(); // Типы закладок отчетов 
        accordion_type.initObject(); // Типы закладок отчетов 

        //var dd = allVars.length;
        if (allVars.num != null) {
            svagon.spinner("value", allVars.num);
            table_vagon.viewTables(true); // Первый запуск
        }


    }); // локализация



});
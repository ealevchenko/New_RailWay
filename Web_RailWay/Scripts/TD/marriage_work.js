$(function () {

    // Список общесистемных слов 
    $.Text_View =
        {
            'default':  //default language: ru
            {
                'text_link_tabs_report_1': 'Список брака',
                'text_link_tabs_report_2': 'Отчеты',
                'field_DateStarted': 'Дата и время',
                'field_TankNo': '№ Бака',
                'field_OilType': 'Тип Масла',
                'field_Invent': 'Инв. №',
                'field_Receiver': 'Получатель',
                'field_TargetVolume': 'Объем (м3)',
                'field_CreatedDens': 'Плотность (кг/м3)',
                'field_TargetMass': 'Масса (т)',
                'bt_left_title': 'Предыдущая дата',
                'bt_right_title': 'Следующая дата',
                'bt_refresh_title': 'Обновить отчет',
                'bt_refresh_text': 'Обновить отчет',
                'label_select_date': 'Выберите дату',
                'select_text_sm1': 'Смена Д (07:00-18:59)',
                'select_text_sm2': 'Смена Н (19:00-06:59)',
            },
            'en':  //default language: English
            {
                'text_link_tabs_report_1': 'Marriage list',
                'text_link_tabs_report_2': 'Report',
                'field_DateStarted': 'Date and time',
                'field_TankNo': 'Tanks No.',
                'field_OilType': 'Type Oil',
                'field_Invent': 'Inventory No.',
                'field_Receiver': 'Recipient',
                'field_TargetVolume': 'Volume(m3)',
                'field_CreatedDens': 'Density(kg/m3)',
                'field_TargetMass': 'Mass(t)',
                'bt_left_title': 'Previous Date',
                'bt_right_title': 'Next Date',
                'bt_refresh_title': 'Refresh Report',
                'bt_refresh_text': 'Refresh Report',
                'label_select_date': 'Select a date',
                'select_text_sm1': 'Shift Day (07:00-18:59)',
                'select_text_sm2': 'Shift Night (19:00-06:59)',
            }
        };

    var lang = $.cookie('lang'),
        date_curent = new Date(),
        date_start = null,
        date_stop = null,
        langs = $.extend(true, $.extend(true, getLanguages($.Text_View, lang), getLanguages($.Text_Common, lang)), getLanguages($.Text_Table, lang)),
        //// Загрузка библиотек
        //loadReference = function (callback) {
        //    LockScreen(langView('mess_load', langs));
        //    var count = 1;
        //    // Загрузка списка карточек (common.js)
        //    getReference_azsCards(function (result) {
        //        reference_cards = result;
        //        count -= 1;
        //        if (count <= 0) {
        //            if (typeof callback === 'function') {
        //                LockScreenOff();
        //                callback();
        //            }
        //        }
        //    })
        //},
        // список карточек
        //reference_cards = null,
        //// Типы отчетов
        tab_type_reports = {
            html_div: $("#tabs-reports"),
            active: 0,
            initObject: function () {
                $('#link-tabs-report-1').text(langView('text_link_tabs_report_1', langs));
                $('#link-tabs-report-2').text(langView('text_link_tabs_report_2', langs));
                this.html_div.tabs({
                    collapsible: true,
                    activate: function (event, ui) {
                        tab_type_reports.active = tab_type_reports.html_div.tabs("option", "active");
                        //tab_type_reports.activeTable(tab_type_cards.active, false);
                    },
                });
                //this.activeTable(this.active, true);
            },
            activeTable: function (active, data_refresh) {
                if (active === 0) {
                    table_report.viewTable(data_refresh);
                }
                //if (active == 1) {
                //    table_report.viewTable(data_refresh);
                //}

            },

        },
        // Панель таблицы
        panel_select_report = {
            html_div_panel: $('#table-panel'),
            obj: null,
            obj_date: null,
            bt_left: $('<button class="ui-button ui-widget ui-corner-all ui-button-icon-only" ><span class="ui-icon ui-icon-circle-triangle-w"></span>text</button>'),
            bt_right: $('<button class="ui-button ui-widget ui-corner-all ui-button-icon-only" ><span class="ui-icon ui-icon-circle-triangle-e"></span>text</button>'),
            bt_refresh: $('<button class="ui-button ui-widget ui-corner-all" ><span class="ui-icon ui-icon-refresh"></span>text</button>'),
            label: $('<label for="date" ></label>'),
            span: $('<span id="select-range"></span>'),
            input_date: $('<input id="date" name="date" size="20">'),
            select_sm: $('<select class="ui-widget-content ui-corner-all"></select>'),
            initObject: function () {
                this.span.append(this.input_date);
                obj = this.html_div_panel;
                obj
                    //.append(this.bt_left)
                    .append(this.label)
                    .append(this.span)
                    //.append(this.bt_right)
                    .append(this.select_sm)
                    .append(this.bt_refresh);
                //this.bt_left.attr('title',(langView('bt_left_title', langs)));
                this.label.text(langView('label_select_date', langs));
                //this.bt_right.attr('title',langView('bt_right_title', langs));
                this.bt_refresh.attr('title', langView('bt_refresh_title', langs));
                this.bt_refresh.text(langView('bt_refresh_text', langs));

                this.bt_refresh.on('click', function () {
                    panel_select_report.viewReport();
                });

                // Настроим выбор времени
                initSelect(
                    this.select_sm,
                    { width: 200 },
                    [{ value: 1, text: langView('select_text_sm1', langs) }, { value: 2, text: langView('select_text_sm2', langs) }],
                    null,
                    1,
                    function (event, ui) {
                        event.preventDefault();
                        // Обработать выбор смены
                        panel_select_report.viewReport();
                    },
                    null);
                // настроим компонент выбора времени
                this.obj_date = this.input_date.dateRangePicker(
                    {
                        startOfWeek: 'monday',
                        //separator: lang == 'en' ? 'to' : 'по',
                        language: lang === undefined ? 'ru' : lang,
                        format: lang === 'en' ? 'MM/DD/YYYY' : 'DD.MM.YYYY',
                        autoClose: true,
                        singleDate: true,
                        showShortcuts: false,
                    }).
                    bind('datepicker-change', function (evt, obj) {
                        date_curent = obj.date1;
                    })
                    .bind('datepicker-closed', function () {
                        panel_select_report.viewReport();
                    });
                // Выставим текущую дату
                var date_curent_set = date_curent.getDate() + '.' + (date_curent.getMonth() + 1) + '.' + date_curent.getFullYear() + ' 00:00';
                this.obj_date.data('dateRangePicker').setDateRange(date_curent_set, date_curent_set, true);
            },
            viewReport: function () {
                if (panel_select_report.select_sm.val() === "2") {
                    date_start = new Date(date_curent.getFullYear(), date_curent.getMonth(), date_curent.getDate(), 19, 0, 0);
                    date_stop = new Date(date_curent.getFullYear(), date_curent.getMonth(), date_curent.getDate() + 1, 6, 59, 59);
                }
                if (panel_select_report.select_sm.val() === "1") {
                    date_start = new Date(date_curent.getFullYear(), date_curent.getMonth(), date_curent.getDate(), 7, 0, 0);
                    date_stop = new Date(date_curent.getFullYear(), date_curent.getMonth(), date_curent.getDate(), 18, 59, 59);
                }
                //tab_type_reports.activeTable(tab_type_reports.active, true);
            }
        },
        // Таблица 
        table_report = {
            obj_table: null,
            select: null,
            select_id: null,
            list: [],
            // Инициализировать таблицу
            initObject: function () {
                this.obj = $('table#table-report').DataTable({
                    //"lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                    "paging": false,
                    "ordering": true,
                    "info": false,
                    "select": true,
                    "autoWidth": false,
                    //"filter": true,
                    //"scrollY": "600px",
                    //"scrollX": true,
                    language: language_table(langs),
                    jQueryUI: true,
                    "createdRow": function (row, data, index) {
                        $(row).attr('id', data.id);
                    },
                    //"footerCallback": function (row, data, start, end, display) {
                    //    var api = this.api(), data;
                    //    // Remove the formatting to get integer data for summation
                    //    var intVal = function (i) {
                    //        return typeof i === 'string' ?
                    //            i.replace(/[\$,]/g, '') * 1 :
                    //            typeof i === 'number' ?
                    //            i : 0;
                    //    };
                    //    // Total volume
                    //    total_dt_volume = api
                    //        .data()
                    //        .reduce(function (a, b) {
                    //            if (b.Fuel == 107000024) {
                    //                return intVal(a) + intVal(b.UsageVolume);
                    //            } else { return intVal(a);}
                    //        }, 0);
                    //    total_a92_volume = api
                    //        .data()
                    //        .reduce(function (a, b) {
                    //            if (b.Fuel == 107000022) {
                    //                return intVal(a) + intVal(b.UsageVolume);
                    //            } else { return intVal(a);}
                    //        }, 0);
                    //    total_a95_volume = api
                    //        .data()
                    //        .reduce(function (a, b) {
                    //            if (b.Fuel == 107000023) {
                    //                return intVal(a) + intVal(b.UsageVolume);
                    //            } else { return intVal(a);}
                    //        }, 0);
                    //    total_kerosin_volume = api
                    //        .data()
                    //        .reduce(function (a, b) {
                    //            if (b.Fuel == 107000027) {
                    //                return intVal(a) + intVal(b.UsageVolume);
                    //            } else { return intVal(a);}
                    //        }, 0);
                    //    //total_volume = api
                    //    //    .column(6)
                    //    //    .data()
                    //    //    .reduce(function (a, b) {
                    //    //        return intVal(a) + intVal(b);
                    //    //    }, 0);
                    //    // Total mass
                    //    total_dt_mass = api
                    //        .data()
                    //        .reduce(function (a, b) {
                    //            if (b.Fuel == 107000024) {
                    //                return intVal(a) + intVal(b.UsageMass);
                    //            } else { return intVal(a);}
                    //        }, 0);
                    //    total_a92_mass = api
                    //        .data()
                    //        .reduce(function (a, b) {
                    //            if (b.Fuel == 107000022) {
                    //                return intVal(a) + intVal(b.UsageMass);
                    //            } else { return intVal(a);}
                    //        }, 0);
                    //    total_a95_mass = api
                    //        .data()
                    //        .reduce(function (a, b) {
                    //            if (b.Fuel == 107000023) {
                    //                return intVal(a) + intVal(b.UsageMass);
                    //            } else { return intVal(a);}
                    //        }, 0);
                    //    total_kerosin_mass = api
                    //        .data()
                    //        .reduce(function (a, b) {
                    //            if (b.Fuel == 107000027) {
                    //                return intVal(a) + intVal(b.UsageMass);
                    //            } else { return intVal(a);}
                    //        }, 0);
                    //    //total_mass = api
                    //    //    .column(7)
                    //    //    .data()
                    //    //    .reduce(function (a, b) {
                    //    //        return intVal(a) + intVal(b);
                    //    //    }, 0);


                    //    $('#a92-volume').text(total_a92_volume.toFixed(2)+' (л)');
                    //    $('#a95-volume').text(total_a95_volume.toFixed(2)+' (л)');
                    //    $('#dt-volume').text(total_dt_volume.toFixed(2)+' (л)');
                    //    $('#kerosin-volume').text(total_kerosin_volume.toFixed(2)+' (л)');
                    //    // Update footer mass
                    //    $('#a92-mass').text(total_a92_mass.toFixed(2) + ' (кг)');
                    //    $('#a95-mass').text(total_a95_mass.toFixed(2) + ' (кг)');
                    //    $('#dt-mass').text(total_dt_mass.toFixed(2) + ' (кг)');
                    //    $('#kerosin-mass').text(total_kerosin_mass.toFixed(2) + ' (кг)');
                    //},
                    columns: [

                        { data: "DateStarted", title: langView('field_DateStarted', langs), width: "150px", orderable: true, searchable: false },
                        //{ data: "TankNo", title: langView('field_TankNo', langs), width: "50px", orderable: true, searchable: true },
                        //{ data: "OilType", title: langView('field_OilType', langs), width: "50px", orderable: true, searchable: true },
                        //{ data: "Invent", title: langView('field_Invent', langs), width: "100px", orderable: true, searchable: true },
                        //{ data: "Receiver", title: langView('field_Receiver', langs), width: "50px", orderable: false, searchable: false },
                        //{ data: "TargetVolume", title: langView('field_TargetVolume', langs), width: "50px", orderable: false, searchable: false },
                        //{ data: "CreatedDens", title: langView('field_CreatedDens', langs), width: "50px", orderable: false, searchable: false },
                        //{ data: "TargetMass", title: langView('field_TargetMass', langs), width: "50px", orderable: false, searchable: false },
                    ],
                });
            },
            // Показать таблицу с данными
            viewTable: function (data_refresh) {
                LockScreen(langView('mess_delay', langs));
                if (this.list === null | data_refresh === true) {
                    // Обновим данные
                    getAsyncViewOilSalesOfDateTime(
                        date_start, date_stop,
                        function (result) {
                            table_report.list = result;
                            table_report.loadDataTable(result);
                            table_report.obj.draw();
                        }
                    );
                } else {
                    table_report.loadDataTable(this.list);
                    table_report.obj.draw();
                };
            },
            // Загрузить данные
            loadDataTable: function (data) {
                this.list = data;
                this.obj.clear();
                for (i = 0; i < data.length; i++) {
                    this.obj.row.add({
                        //"Id": data[i].Id,
                        //"DateStarted": data[i].DateStarted,
                        //"TankNo": data[i].TankNo,
                        //"OilType": data[i].OilType,
                        //"Invent": data[i].Invent,
                        //"Receiver": data[i].Receiver,
                        //"TargetVolume": data[i].TargetVolume !== null ? (data[i].TargetVolume / 1000).toFixed(3) : null,
                        //"CreatedDens": data[i].CreatedDens !== null ? data[i].CreatedDens.toFixed(1) : null,
                        //"TargetMass": data[i].TargetMass !== null ? (data[i].TargetMass / 1000).toFixed(3) : null,
                    });
                }
                LockScreenOff();
            },
        };

    //-----------------------------------------------------------------------------------------
    // Функции
    //-----------------------------------------------------------------------------------------

    //-----------------------------------------------------------------------------------------
    // Инициализация объектов
    //-----------------------------------------------------------------------------------------
    panel_select_report.initObject();
    tab_type_reports.initObject();
    //// Загрузка библиотек
    //loadReference(function (result) {
    table_report.initObject();
    panel_select_report.viewReport();
    //});

});
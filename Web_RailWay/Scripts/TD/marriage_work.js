// Контроль нажатия кнопки на клавиатуре (исключить сворачивание окон по нажатию "ENTER")
$(document).keypress(
    function (event) {

        if (event.which === 13) {
            $(".validateTips").text('');
            $(".ui-state-error").removeClass("ui-state-error");
            event.preventDefault();

        }
    });

$(function () {

    // Список общесистемных слов 
    $.Text_View =
        {
            'default':  //default language: ru
            {
                'text_link_tabs_marriage_table': 'Браки в работе',
                'text_link_tabs_marriage_report': 'Отчеты',
                'field_date_start': 'Дата, время инцидента',
                'field_date_stop': 'Дата, время устранения инцидента',
                'field_date_period': 'Дата и время начала и устранения инцидента',
                'field_district_object': 'Место нарушения (станция, пост, перегон)',
                'field_site': 'Участок нарушения (путь, СП)',
                'field_classification': 'Классификация нарушения',
                'field_nature': 'Характер нарушения',
                'field_classification_nature': 'Классификация и характер нарушения',
                'field_num': '№ подвижного состава',
                'field_cause': 'Причина нарушения',
                'field_type_cause': 'Техническая / организационная',
                'field_cause_type_cause': 'Причина нарушения',
                'field_subdivision': 'Подразделение',
                'field_akt': '№ акта',
                'field_locomotive_series': 'Серия, № локомотива',
                'field_driver': 'Машинист',
                'field_helper': 'Помощник, составитель',
                'field_measures': 'Принятые меры',
                'field_note': 'Примечание',
                'field_create': 'Запись создана',

                'label-text-date': 'Выберите период',
                'title_insert_marriage': 'Добавить запись брак в работе',
                'title_edit_marriage': 'Править запись брак в работе',
                'title_delete_marriage': 'Удалить запись брак в работе',
                'button_insert': 'Добавить',
                'button_edit': 'Править',
                'button_delete': 'Удалить',
                //'legend_delete_text': 'Вы действительно хотите удалить эту RFID-карту?'
            },
            'en':  //default language: English
            {
                'text_link_tabs_marriage_table': 'Marriages at work',
                'text_link_tabs_marriage_report': 'Reports',
                'field_date_start': 'Date, time of incident',
                'field_date_stop': 'Date, time to eliminate incident',
                'field_date_period': 'Date and time of the beginning and elimination of the incident',
                'field_district_object': 'Location of the violation (station, fasting, driving)',
                'field_site': 'Violation site (path, SP)',
                'field_classification': 'Violation classification',
                'field_nature': 'Nature of the violation',
                'field_classification_nature': 'Classification and nature of the violation',
                'field_num': 'rolling stock no.',
                'field_cause': 'Cause of violation',
                'field_type_cause': 'Technical / Organizational',
                'field_cause_type_cause': 'Cause of violation',
                'field_subdivision': 'Subdivision',
                'field_akt': 'Act No.',
                'field_locomotive_series': 'Series, locomotive number',
                'field_driver': 'Machinist',
                'field_helper': 'Helper, compiler',
                'field_measures': 'Measures taken',
                'field_note': 'Note',
                'field_create': 'Record created',

                'label-text-date': 'Select period',
                'title_insert_marriage': 'Add a defect in work entry',
                'title_edit_marriage': 'Edit the record marriage at work',
                'title_delete_marriage': 'Delete the entry marriage in work',
                'button_insert': 'Add',
                'button_edit': 'Edit',
                'button_delete': 'Delete',
                //'legend_delete_text': 'Do you really want to delete this RFID card?'
            }
        };

    var lang = $.cookie('lang') === undefined ? 'ru' : $.cookie('lang'),
        langs = $.extend(true, $.extend(true, getLanguages($.Text_View, lang), getLanguages($.Text_Common, lang)), getLanguages($.Text_Table, lang)),
        //function_delete_cards_b = Boolean(Number(function_delete_cards)),
        //// Загрузка библиотек
        //loadReference = function (callback) {
        //    LockScreen(langView('mess_load', langs));
        //    var count = 5;
        //    // Загрузка списка (dt.js)
        //    getAsyncDTMarriageDistrictObject(function (result) {
        //        reference_district_object = result;
        //        count -= 1;
        //        if (count <= 0) {
        //            if (typeof callback === 'function') {
        //                LockScreenOff();
        //                callback();
        //            }
        //        }
        //    });
        //    getAsyncDTMarriageClassification(function (result) {
        //        reference_classification = result;
        //        count -= 1;
        //        if (count <= 0) {
        //            if (typeof callback === 'function') {
        //                LockScreenOff();
        //                callback();
        //            }
        //        }
        //    });
        //    getAsyncDTMarriageNature(function (result) {
        //        reference_nature = result;
        //        count -= 1;
        //        if (count <= 0) {
        //            if (typeof callback === 'function') {
        //                LockScreenOff();
        //                callback();
        //            }
        //        }
        //    });
        //    getAsyncDTMarriageCause(function (result) {
        //        reference_cause = result;
        //        count -= 1;
        //        if (count <= 0) {
        //            if (typeof callback === 'function') {
        //                LockScreenOff();
        //                callback();
        //            }
        //        }
        //    });
        //    getAsyncDTMarriageSubdivision(function (result) {
        //        reference_subdivision = result;
        //        count -= 1;
        //        if (count <= 0) {
        //            if (typeof callback === 'function') {
        //                LockScreenOff();
        //                callback();
        //            }
        //        }
        //    });
        //},
        // список place
        //reference_district_object = null,
        //// список classification
        //reference_classification = null,
        //// список nature
        //reference_nature = null,
        //// список cause
        //reference_cause = null,
        //// список subdivision
        //reference_subdivision = null,
        // Типы отчетов
        tab_type_report = {
            html_div: $("div#tabs-reports"),
            active: 0,
            initObject: function () {
                $('#link-tab-report-1').text(langView('text_link_tabs_marriage_table', langs));
                $('#link-tab-report-2').text(langView('text_link_tabs_marriage_report', langs));
                this.html_div.tabs({
                    collapsible: true,
                    activate: function (event, ui) {
                        tab_type_report.active = tab_type_report.html_div.tabs("option", "active");
                        //tab_type_report.activeTable(tab_type_report.active, false);
                    },
                });
                //this.activeTable(this.active, true);
            },
            activeTable: function (active, data_refresh) {
                if (active === 0) {
                    table_report_1.viewTable(data_refresh);
                }
                //if (active === 1) {
                //    table_sending.viewTable(data_refresh);
                //}

            },

        },
        // Панель таблицы
        panel_table_1 = {
            date_start: new Date(new Date().getFullYear(), new Date().getMonth(), 1, 0, 0, 0),
            date_stop: new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate(), 23, 59, 59),
            period: null,
            obj_date: null,
            html_div_panel: $('<div class="setup-operation" id="property"></div>'),
            label: $('<label for="date" ></label>'),
            span: $('<span id="select-range"></span>'),
            input_data_start: $('<input id="date-start" name="date-start" size="20">'),
            input_data_stop: $('<input id="date-stop" name="date-stop" size="20">'),
            initPanel: function (obj) {
                this.html_div_panel
                    .append(this.label.text(langView('label-text-date', langs)))
                    .append(this.span.append(this.input_data_start).append(' - ').append(this.input_data_stop));

                //}
                obj.prepend(this.html_div_panel);
                // настроим компонент выбора времени
                this.obj_date = this.span.dateRangePicker(
                    {
                        language: lang,
                        format: lang === 'en' ? 'MM/DD/YYYY HH:mm' : 'DD.MM.YYYY HH:mm',
                        separator: lang === 'en' ? '-' : '-',
                        autoClose: false,
                        time: {
                            enabled: true
                        },
                        setValue: function (s, s1, s2) {
                            $('input#date-start').val(s1);
                            $('input#date-stop').val(s2);
                            panel_table_1.period = s1 + '-' + s2;
                        }
                    }).
                    bind('datepicker-change', function (evt, obj) {
                        panel_table_1.date_start = obj.date1;
                        panel_table_1.date_stop = obj.date2;
                        panel_table_1.period = obj.value;
                    })
                    .bind('datepicker-closed', function () {
                        tab_type_report.activeTable(tab_type_report.active, false);
                    });
                if (lang === 'en') {
                    this.obj_date.data('dateRangePicker').setDateRange(datetoStringOfLang(this.date_start, 'en'), datetoStringOfLang(this.date_stop, 'en'));

                } else {
                    this.obj_date.data('dateRangePicker').setDateRange(datetoStringOfLang(this.date_start, 'ru'), datetoStringOfLang(this.date_stop, 'ru'));
                }
            }
        },
        // Таблица 
        table_report_1 = {
            html_table: $('table#table-report-1'),
            obj_table: null,
            select: null,
            select_id: null,
            list: [],
            // Инициализировать таблицу
            initObject: function () {
                this.obj = this.html_table.DataTable({
                    "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                    "paging": true,
                    "ordering": true,
                    "info": false,
                    //"select": true,
                    "autoWidth": false,
                    //"filter": true,
                    //"scrollY": "600px",
                    //"scrollX": true,
                    language: language_table(langs),
                    jQueryUI: true,
                    "createdRow": function (row, data, index) {
                        $(row).attr('id', data.id);
                        //table_report_1.viewUpdateActive(row, data.Active)
                    },
                    columns: [
                        { data: "date_start", title: langView('field_date_start', langs), width: "100px", orderable: true, searchable: true },
                        { data: "date_stop", title: langView('field_date_stop', langs), width: "100px", orderable: true, searchable: true },
                        { data: "district_object", title: langView('field_district_object', langs), width: "100px", orderable: true, searchable: true },
                        { data: "site", title: langView('field_site', langs), width: "100px", orderable: true, searchable: true },
                        { data: "classification", title: langView('field_classification', langs), width: "100px", orderable: true, searchable: true },
                        { data: "nature", title: langView('field_nature', langs), width: "100px", orderable: true, searchable: true },
                        { data: "num", title: langView('field_num', langs), width: "100px", orderable: true, searchable: true },
                        { data: "cause", title: langView('field_cause', langs), width: "100px", orderable: true, searchable: true },
                        { data: "type_cause", title: langView('field_type_cause', langs), width: "50px", orderable: true, searchable: true },
                        { data: "subdivision", title: langView('field_subdivision', langs), width: "100px", orderable: true, searchable: true },
                        { data: "akt", title: langView('field_akt', langs), width: "100px", orderable: true, searchable: true },
                        { data: "locomotive_series", title: langView('field_locomotive_series', langs), width: "100px", orderable: true, searchable: true },
                        { data: "driver", title: langView('field_driver', langs), width: "100px", orderable: true, searchable: true },
                        { data: "helper", title: langView('field_helper', langs), width: "100px", orderable: true, searchable: true },
                        { data: "measures", title: langView('field_measures', langs), width: "100px", orderable: true, searchable: true },
                        { data: "note", title: langView('field_note', langs), width: "100px", orderable: true, searchable: true },
                    ],
                });
                this.initEventSelect();
                panel_table_1.initPanel($('DIV#table-panel'));
            },
            // Показать таблицу с данными
            viewTable: function (data_refresh) {
                LockScreen(langView('mess_delay', langs));
                if (this.list === null || this.period !== panel_table_1.period || data_refresh === true) {
                    // Обновим данные
                    getAsyncDTMarriageOfDate(
                        panel_table_1.date_start,
                        panel_table_1.date_stop,
                        function (result) {
                            table_report_1.list = result;
                            table_report_1.period = panel_table_1.period;
                            table_report_1.loadDataTable(result);
                            table_report_1.obj.draw();
                        }
                    );
                } else {
                    table_report_1.loadDataTable(this.list);
                    table_report_1.obj.draw();
                }
            },
            // Загрузить данные
            loadDataTable: function (data) {
                this.list = data;
                table_report_1.obj.clear();
                for (i = 0; i < data.length; i++) {
                    this.obj.row.add({
                        "id": data[i].id,
                        "date_start": data[i].date_start,
                        "date_stop": data[i].date_stop,
                        "id_district_object": data[i].id_district_object,
                        "district_object": data[i].MarriageDistrictObject !== null ? data[i].MarriageDistrictObject.district_object : null,
                        "site": data[i].site,
                        "id_classification": data[i].id_classification,
                        "classification": data[i].MarriageClassification !== null ? data[i].MarriageClassification.classification : null,
                        "id_nature": data[i].id_nature,
                        "nature": data[i].MarriageNature !== null ? data[i].MarriageNature.nature : null,
                        "num": data[i].num,
                        "id_cause": data[i].id_cause,
                        "cause": data[i].MarriageCause !== null ? data[i].MarriageCause.cause : null,
                        "id_type_cause": data[i].id_type_cause,
                        "type_cause": data[i].id_type_cause === 0 ? "О" : "Т",
                        "id_subdivision": data[i].id_subdivision,
                        "subdivision": data[i].MarriageSubdivision !== null ? data[i].MarriageSubdivision.subdivision : null,
                        "akt": data[i].akt,
                        "locomotive_series": data[i].locomotive_series,
                        "driver": data[i].driver,
                        "helper": data[i].helper,
                        "measures": data[i].measures,
                        "note": data[i].note,
                        "create": data[i].create,
                        "create_user": data[i].create_user,
                        "change": data[i].change,
                        "change_user": data[i].change_user,
                    });
                }
                LockScreenOff();
            },
            // Инициализация события выбора поля
            initEventSelect: function () {
                this.html_table.find('tbody')
                    .on('click', 'tr', function () {
                        var id_select = $(this).attr('id');
                        table_report_1.select_id = id_select !== null ? Number(id_select) : null;
                        var select = getObjects(table_report_1.list, 'id', table_report_1.select_id);
                        if (select !== null && select.length > 0) {
                            table_report_1.select = select[0];
                        };
                        table_report_1.clearSelect();
                        $(this).addClass('selected');
                    });
            },
            // Сбросить выбор поля
            clearSelect: function () {
                this.html_table.find('tbody tr').removeClass('selected');
            },
        };
    //-----------------------------------------------------------------------------------------
    // Функции
    //-----------------------------------------------------------------------------------------
    // Вывести сообщение на основной экран
    var updateMessageTips = function (t) {
        $(".messageTips")
            .text(t)
            .addClass("ui-state-highlight");
        setTimeout(function () {
            $(".messageTips").removeClass("ui-state-highlight", 1500);
        }, 500);
    };
    //-----------------------------------------------------------------------------------------
    // Инициализация объектов
    //-----------------------------------------------------------------------------------------
    tab_type_report.initObject();
    // Загрузка библиотек
    //loadReference(function (result) {
        table_report_1.initObject();
        tab_type_report.activeTable(tab_type_report.active, true);
    //});

});
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
                'field_place': 'Место нарушения (станция, пост, перегон)',
                'field_site': 'Участок нарушения (путь, СП)',
                'field_classification': 'Классификация нарушения',
                'field_nature': 'Характер нарушения',
                'field_num': '№ подвижного состава',
                'field_cause': 'Причина нарушения',
                'field_type_cause': 'Техническая / организационная',
                'field_subdivision': 'Подразделение',
                'field_akt': '№ акта',
                'field_locomotive_series': 'Серия, № локомотива',
                'field_driver': 'Машинист',
                'field_helper': 'Помощник, составитель',
                'field_measures': 'Принятые меры',
                'field_note': 'Примечание',

                'label-text-date': 'Выберите период',
                'title_insert_marriage': 'Добавить карту',
                'title_edit_marriage': 'Править карту',
                'title_delete_marriage': 'Удалить карту',
                'button_insert': 'Добавить',
                'button_edit': 'Править',
                'button_delete': 'Удалить',
                'legend_delete_text': 'Вы действительно хотите удалить эту RFID-карту?'

            },
            'en':  //default language: English
            {
                'text_link_tabs_marriage_table': 'RFID-Cards',
                'text_link_tabs_marriage_report': 'Users',
                'field_date_start': 'Date, time of incident',
                'field_date_stop': 'Date, time of incident elimination',
                'field_place': 'Place of violation (station, post, stage)',
                'field_site': 'Site of violation (path, joint venture)',
                'field_classification': 'Classification of violation',
                'field_nature': 'Nature of the violation',
                'field_num': 'No. of rolling stock',
                'field_cause': 'Reason for violation',
                'field_type_cause': 'Technical / Organizational',
                'field_subdivision': 'Subdivision',
                'field_akt': 'Act No.',
                'field_locomotive_series': 'Series, locomotive number',
                'field_driver': 'Machinist',
                'field_helper': 'Helper, compiler',
                'field_measures': 'Measures taken',
                'field_note': 'Note',

                'label-text-date': 'Select a date',
                'title_insert_marriage': 'Add a card',
                'title_edit_marriage': 'Edit card',
                'title_delete_marriage': 'Delete Card',
                'button_insert': 'Add',
                'button_edit': 'Edit',
                'button_delete': 'Delete',
                'legend_delete_text': 'Do you really want to delete this RFID card?'
            }
        };

    var lang = $.cookie('lang') === undefined ? 'ru' : $.cookie('lang'),
        langs = $.extend(true, $.extend(true, getLanguages($.Text_View, lang), getLanguages($.Text_Common, lang)), getLanguages($.Text_Table, lang)),
        //function_delete_cards_b = Boolean(Number(function_delete_cards)),
        // Загрузка библиотек
        loadReference = function (callback) {
            LockScreen(langView('mess_load', langs));
            var count = 5;
            // Загрузка списка (dt.js)
            getAsyncDTMarriagePlace(function (result) {
                reference_place = result;
                count -= 1;
                if (count <= 0) {
                    if (typeof callback === 'function') {
                        LockScreenOff();
                        callback();
                    }
                }
            });
            getAsyncDTMarriageClassification(function (result) {
                reference_classification = result;
                count -= 1;
                if (count <= 0) {
                    if (typeof callback === 'function') {
                        LockScreenOff();
                        callback();
                    }
                }
            });
            getAsyncDTMarriageNature(function (result) {
                reference_nature = result;
                count -= 1;
                if (count <= 0) {
                    if (typeof callback === 'function') {
                        LockScreenOff();
                        callback();
                    }
                }
            });
            getAsyncDTMarriageCause(function (result) {
                reference_cause = result;
                count -= 1;
                if (count <= 0) {
                    if (typeof callback === 'function') {
                        LockScreenOff();
                        callback();
                    }
                }
            });
            getAsyncDTMarriageSubdivision(function (result) {
                reference_subdivision = result;
                count -= 1;
                if (count <= 0) {
                    if (typeof callback === 'function') {
                        LockScreenOff();
                        callback();
                    }
                }
            });
        },
        // список place
        reference_place = null,
        // список classification
        reference_classification = null,
        // список nature
        reference_nature = null,
        // список cause
        reference_cause = null,
        // список subdivision
        reference_subdivision = null,
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
            button_insert: $('<button class="ui-button ui-widget ui-corner-all"></button>'),
            button_edit: $('<button class="ui-button ui-widget ui-corner-all"></button>'),
            button_delete: $('<button class="ui-button ui-widget ui-corner-all"></button>'),
            label: $('<label for="date" ></label>'),
            span: $('<span id="select-range"></span>'),
            input_data_start: $('<input id="date-start" name="date-start" size="20">'),
            input_data_stop: $('<input id="date-stop" name="date-stop" size="20">'),
            initPanel: function (obj) {
                this.html_div_panel
                    .append(this.label.text(langView('label-text-date', langs)))
                    .append(this.span.append(this.input_data_start).append(' - ').append(this.input_data_stop))
                    .append(this.button_insert.text(langView('button_insert', langs)))
                    .append(this.button_edit.text(langView('button_edit', langs)).hide());
                //if (function_delete_cards_b) {
                this.html_div_panel.append(this.button_delete.text(langView('button_delete', langs)).hide());
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
                // Обработка события
                this.button_insert.on('click', function () {
                    confirm_ins_edit_panel.Open(null);
                });
                this.button_edit.on('click', function () {

                    confirm_ins_edit_panel.Open(table_report_1.select_id);
                });
                this.button_delete.on('click', function () {
                    confirm_delete_panel.Open(table_report_1.select_id);
                });
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
                    "select": true,
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
                        { data: "place", title: langView('field_place', langs), width: "100px", orderable: true, searchable: true },
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
                        "id_place": data[i].id_place,
                        "place": data[i].MarriagePlace !== null ? data[i].MarriagePlace.place : null,
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
                        panel_table_1.button_edit.show();
                        panel_table_1.button_delete.show();
                    });
            },
            // Сбросить выбор поля
            clearSelect: function () {
                this.html_table.find('tbody tr').removeClass('selected');
            },
            // Обновим
            viewUpdateString: function (data) {
                if (data != null) {
                    var row_ind = table_report_1.obj.row('#' + data.Id).index();
                    table_report_1.obj.cell(row_ind, 1).data(data.Number);
                    table_report_1.obj.cell(row_ind, 2).data(data.AutoNumber);
                    table_report_1.obj.cell(row_ind, 3).data(data.Debitor);
                    table_report_1.obj.cell(row_ind, 4).data(data.Sn1);
                    table_report_1.obj.cell(row_ind, 5).data(data.AutoModel);
                    table_report_1.obj.cell(row_ind, 6).data(data.Street);
                    table_report_1.obj.cell(row_ind, 7).data(reference_departs !== null ? reference_departs.getText(data.House) : data.House);
                    table_report_1.viewUpdateActive(table_report_1.html_table.find('tbody tr.selected'), data.Active);
                    table_report_1.obj.draw(false);
                }
            },
            // добавить
            viewInsertString: function (data) {
                if (data != null) {
                    this.obj.row.add({
                        "id": data.Id,
                        "Number": data.Number,
                        "DriverName": data.DriverName,
                        "AutoNumber": data.AutoNumber,
                        "Debitor": data.Debitor,
                        "Sn1": data.Sn1,
                        "Sn2": data.Sn2,
                        "AutoModel": data.AutoModel,
                        "Street": data.Street,
                        "House": data.House,
                        "CreateDate": data.CreateDate,
                        "CreateTime": data.CreateTime,
                        "UpdateTime": data.UpdateTime,
                        "Owner": data.Owner,
                        "UpdateDate": data.UpdateDate,
                        "Active": data.Active,
                        "HouseName": reference_departs != null ? reference_departs.getText(data.House) : data.House,
                    });
                    table_report_1.obj.draw();
                }
            },
            // удалить
            viewDeleteString: function (data) {
                if (data != null) {
                    table_report_1.obj.row('.selected').remove().draw(false);
                }
            },
            // Показать поле активности
            viewUpdateActive: function (row, active) {
                if (active == null) {
                    $('td', row).eq(8).html("").removeClass().addClass('null_active');
                } else {
                    if (active) { $('td', row).eq(8).html("").removeClass().addClass('active'); }
                    else { $('td', row).eq(8).html("").removeClass().addClass('not_active'); }
                }
            },

        },
        // Панель добавить обновить карту
        confirm_ins_edit_panel = {
            html_div: $("#ins-edit-confirm"),
            id: 0,
            user: null,
            create_date: null,
            create_user: null,
            dt_start: null,
            dt_stop: null,
            confirm_date_start: null,
            confirm_date_stop: null,
            confirm_place: null,
            confirm_site: null,
            confirm_classification: null,
            confirm_nature: null,
            confirm_num: null,
            confirm_cause: null,
            confirm_type_cause: null,
            confirm_subdivision: null,
            confirm_akt: null,
            confirm_locomotive_series: null,
            confirm_: null,
            confirm_driver: null,
            confirm_helper: null,
            confirm_measures: null,
            confirm_note: null,
            //
            updateTips: function (t) {
                $(".validateTips")
                    .text(t)
                    .addClass("ui-state-highlight");
                setTimeout(function () {
                    $(".validateTips").removeClass("ui-state-highlight", 1500);
                }, 500);
            },
            // Проверка заполнения даты
            checkDateOfMessage: function (o, message) {
                var value = o.val();
                var res1 = /^[0-9]{1,2}[\/\.][0-9]{1,2}[\/\.][0-9]{4} [0-9]{1,2}[\:][0-9]{1,2}$/i.test(o.val());
                var res2 = /^[0-9]{1,2}[\/\.][0-9]{1,2}[\/\.][0-9]{4} [0-9]{1,2}[\:][0-9]{1,2}[\:][0-9]{1,2}$/i.test(o.val());
                if (!res1 && !res2) {
                    o.addClass("ui-state-error");
                    confirm_ins_edit_panel.updateTips(message);
                    return false;
                } else {
                    return true;
                }
            },
            //
            checkLengthOfMessage: function (o, message, min, max) {
                if (o.val().length > max || o.val().length < min) {
                    o.addClass("ui-state-error");
                    confirm_ins_edit_panel.updateTips(message);
                    return false;
                } else {
                    return true;
                }
            },
            //
            checkLength: function (o, n, min, max) {
                if (o.val().length > max || o.val().length < min) {
                    o.addClass("ui-state-error");
                    confirm_ins_edit_panel.updateTips("Размер поля [" + n + "] должен быть в диапазоне от " +
                        min + " до " + max + ".");
                    return false;
                } else {
                    return true;
                }
            },
            //
            checkSelect: function (o, n, min, max) {
                if (o.val() > min && o.val() <= max) {
                    return true;
                } else {
                    o.addClass("ui-state-error");
                    confirm_ins_edit_panel.updateTips("Значение " + n + " должно быть в диапазоне от " +
                        min + " до " + max + ".");
                    return false;
                }
            },
            // Проверка на выбор за указаный период valume
            checkSelectOfMessage: function (o, message, min, max) {
                if (o.val() > max || o.val() < min) {
                    o.addClass("ui-state-error");
                    confirm_ins_edit_panel.updateTips(message);
                    return false;
                } else {
                    return true;
                }
            },
            // Проверка на выбор valume >-1
            checkSelectValOfMessage: function (o, message) {
                //var tst = o.val();
                if (Number(o.val()) < 0) {
                    o.addClass("ui-state-error");
                    confirm_ins_edit_panel.updateTips(message);
                    return false;
                } else {
                    return true;
                }
            },
            // Проверка на пустой объект
            checkIsNullOfMessage: function (o, message) {
                if (o.val() === '' || o.val() === null) {
                    o.addClass("ui-state-error");
                    confirm_ins_edit_panel.updateTips(message);
                    return false;
                } else {
                    return true;
                }
            },
            // Проверка на "0"
            checkIsZeroOfMessage: function (o, message) {
                if (o.val() === 0) {
                    o.addClass("ui-state-error");
                    confirm_ins_edit_panel.updateTips(message);
                    return false;
                } else {
                    return true;
                }
            },
            // Проверка на пустой объект
            checkCheckboxOfMessage: function (o, condition, message) {
                if (o.prop('checked') != condition) {
                    o.addClass("ui-state-error");
                    confirm_ins_edit_panel.updateTips(message);
                    return false;
                } else {
                    return true;
                }
            },
            // Проверка правильного заполнения формы
            validationConfirm: function () {
                var valid = true;
                $(".validateTips").text('');
                $(".ui-state-error").removeClass("ui-state-error");
                // Проверка RFID карты на активность
                valid = valid && confirm_ins_edit_panel.checkDateOfMessage(confirm_ins_edit_panel.confirm_date_start, "Укажите дату и время инцидента");
                valid = valid && confirm_ins_edit_panel.checkSelectValOfMessage(confirm_ins_edit_panel.confirm_place, "Укажите место нарушения");
                valid = valid && confirm_ins_edit_panel.checkIsNullOfMessage(confirm_ins_edit_panel.confirm_site, "Укажите участок нарушения");
                valid = valid && confirm_ins_edit_panel.checkSelectValOfMessage(confirm_ins_edit_panel.confirm_classification, "Укажите классификацию нарушения");
                //valid = valid && confirm_ins_edit_panel.checkSelectValOfMessage(confirm_ins_edit_panel.confirm_nature, "Укажите характер нарушения");
                valid = valid && confirm_ins_edit_panel.checkIsNullOfMessage(confirm_ins_edit_panel.confirm_num, "Укажите номер подвижного состава");
                valid = valid && confirm_ins_edit_panel.checkSelectValOfMessage(confirm_ins_edit_panel.confirm_cause, "Укажите причину нарушения");
                valid = valid && confirm_ins_edit_panel.checkSelectValOfMessage(confirm_ins_edit_panel.confirm_type_cause, "Выберите причину нарушения (техническая\организационная)");
                valid = valid && confirm_ins_edit_panel.checkSelectValOfMessage(confirm_ins_edit_panel.confirm_subdivision, "Укажите подразделение");
                valid = valid && confirm_ins_edit_panel.checkIsNullOfMessage(confirm_ins_edit_panel.confirm_akt, "Укажите номер Акта");
                return valid;
            },
            //
            initObject: function () {
                this.obj = this.html_div.dialog({
                    resizable: false,
                    modal: true,
                    autoOpen: false,
                    height: "auto",
                    width: 800,
                    buttons: {
                        Save: function () {
                            // Проверка формы
                            var valid = confirm_ins_edit_panel.validationConfirm();
                            if (valid) {
                                var marriage_work = confirm_ins_edit_panel.getNewMarriageWork();
                                // Да форма заполнена
                                $(this).dialog("close");
                            } else {
                                // Нет форма не заполнена
                                // .....
                                //alert('valid form');
                            }

                            //var cards = confirm_ins_edit_panel.getNewCards();
                            //if (cards.Id == 0) {
                            //    // Добавить
                            //    postAsyncazsCards(cards,
                            //        function (result) {
                            //            // Проверка на ошибку
                            //            if (result > 0) {
                            //                // ошибки нет
                            //                getAsyncViewazsCardsOfID(result, function (result) {
                            //                    // Обновим
                            //                    table_report_1.viewInsertString(result)
                            //                });
                            //            } else {
                            //                alert('error insert')
                            //            }
                            //        });
                            //} else {
                            //    // Обновить
                            //    putAsyncazsCards(cards,
                            //        function (result) {
                            //            // Проверка на ошибку
                            //            if (result > 0) {
                            //                // ошибки нет
                            //                getAsyncViewazsCardsOfID(confirm_ins_edit_panel.id, function (result) {
                            //                    // Обновим
                            //                    table_report_1.viewUpdateString(result)
                            //                });
                            //            } else {
                            //                alert('error update')
                            //            }
                            //        });
                            //}
                        },
                        Cancel: function () {
                            $(this).dialog("close");
                        }
                    }
                });
                $('th#date-start').text(langView('field_date_start', langs) + ':');
                $('th#date-stop').text(langView('field_date_stop', langs) + ':');
                $('th#place').text(langView('field_place', langs) + ':');
                $('th#site').text(langView('field_site', langs) + ':');
                $('th#classification').text(langView('field_classification', langs) + ':');
                $('th#nature').text(langView('field_nature', langs) + ':');
                $('th#num').text(langView('field_num', langs) + ':');
                $('th#cause').text(langView('field_cause', langs) + ':');
                $('th#type-cause').text(langView('field_type_cause', langs) + ':');
                $('th#subdivision').text(langView('field_subdivision', langs) + ':');
                $('th#akt').text(langView('field_akt', langs) + ':');
                $('th#locomotive-series').text(langView('field_locomotive_series', langs) + ':');
                $('th#driver').text(langView('field_driver', langs) + ':');
                $('th#helper').text(langView('field_helper', langs) + ':');
                $('th#measures').text(langView('field_measures', langs) + ':');
                $('th#note').text(langView('field_note', langs) + ':');
                // настроим компонент выбора времени начала
                this.confirm_date_start = $('input#confirm-date-start').dateRangePicker(
                    {
                        language: lang,
                        format: lang === 'en' ? 'MM/DD/YYYY HH:mm' : 'DD.MM.YYYY HH:mm',
                        autoClose: true,
                        singleDate: true,
                        showShortcuts: false,
                        singleMonth: true,
                        time: {
                            enabled: true
                        }
                    }).
                    bind('datepicker-change', function (evt, obj) {
                        confirm_ins_edit_panel.dt_start = obj.date1;
                    });
                // настроим компонент выбора времени начала
                this.confirm_date_stop = $('input#confirm-date-stop').dateRangePicker(
                    {
                        language: lang,
                        format: lang === 'en' ? 'MM/DD/YYYY HH:mm' : 'DD.MM.YYYY HH:mm',
                        autoClose: true,
                        singleDate: true,
                        showShortcuts: false,
                        singleMonth: true,
                        time: {
                            enabled: true
                        }
                    }).
                    bind('datepicker-change', function (evt, obj) {
                        confirm_ins_edit_panel.dt_stop = obj.date1;
                    });
                // 
                this.confirm_place = initSelect(
                    $('select#confirm-place'),
                    { width: 350 },
                    reference_place,
                    function (row) {
                        return { value: Number(row.id), text: row.place };
                    },
                    -1,
                    function (event, ui) {
                        event.preventDefault();

                    },
                    null);

                this.confirm_site = $('textarea#confirm-site');
                this.confirm_classification = initSelect(
                    $('select#confirm-classification'),
                    { width: 300 },
                    reference_classification,
                    function (row) {
                        return { value: Number(row.id), text: row.classification };
                    },
                    -1,
                    function (event, ui) {
                        event.preventDefault();

                    },
                    null);
                this.confirm_nature = initSelect(
                    $('select#confirm-nature'),
                    { width: 300 },
                    reference_nature,
                    function (row) {
                        return { value: Number(row.id), text: row.nature };
                    },
                    -1,
                    function (event, ui) {
                        event.preventDefault();

                    },
                    null);
                this.confirm_num = $('textarea#confirm-num');
                this.confirm_cause = initSelect(
                    $('select#confirm-cause'),
                    { width: 300 },
                    reference_cause,
                    function (row) {
                        return { value: Number(row.id), text: row.cause };
                    },
                    -1,
                    function (event, ui) {
                        event.preventDefault();

                    },
                    null);
                this.confirm_type_cause = initSelect(
                    $('select#confirm-type-cause'),
                    { width: 300 },
                    [{ value: 0, text: 'О' }, { value: 1, text: 'Т' }],
                    null,
                    -1,
                    function (event, ui) {
                        event.preventDefault();

                    },
                    null);
                this.confirm_subdivision = initSelect(
                    $('select#confirm-subdivision'),
                    { width: 300 },
                    reference_subdivision,
                    function (row) {
                        return { value: Number(row.id), text: row.subdivision };
                    },
                    -1,
                    function (event, ui) {
                        event.preventDefault();

                    },
                    null);
                this.confirm_akt = $('input#confirm-akt');
                this.confirm_locomotive_series = $('textarea#confirm-locomotive-series');
                this.confirm_driver = $('input#confirm-drivers');
                this.confirm_helper = $('input#confirm-helper');
                this.confirm_measures = $('textarea#confirm-measures');
                this.confirm_note = $('textarea#confirm-note');
            },
            //
            Open: function (id) {
                // Сбросим 
                confirm_ins_edit_panel.resetMarriage();
                if (id !== null) {
                    getAsyncDTMarriageOfID(id,
                        function (result_mw) {
                            confirm_ins_edit_panel.setMarriage(result_mw);
                            confirm_ins_edit_panel.obj.dialog("option", "title", langView('title_edit_marriage', langs));
                            confirm_ins_edit_panel.obj.dialog("open");
                        });
                } else {
                    confirm_ins_edit_panel.setMarriage(null);
                    confirm_ins_edit_panel.obj.dialog("option", "title", langView('title_insert_marriage', langs));
                    confirm_ins_edit_panel.obj.dialog("open");
                }
                confirm_ins_edit_panel.obj.dialog("open");
            },
            //
            setMarriage: function (result_mw) {
                confirm_ins_edit_panel.user = $('input#username').val();
                if (result_mw !== null) {
                    this.create_date = result_mw.create;
                    this.create_user = result_mw.create_user;
                    // режим edit
                    confirm_ins_edit_panel.dt_start = new Date(Date.parse(result_mw.date_start));
                    confirm_ins_edit_panel.dt_stop = new Date(Date.parse(result_mw.date_stop));

                    confirm_ins_edit_panel.id = result_mw.id;

                    if (lang === 'en') {
                        this.confirm_date_start.data('dateRangePicker').setDateRange(datetoStringOfLang(confirm_ins_edit_panel.dt_start, 'en'), datetoStringOfLang(confirm_ins_edit_panel.dt_start, 'en'));
                        if (confirm_ins_edit_panel.dt_stop !== null) this.confirm_date_stop.data('dateRangePicker').setDateRange(datetoStringOfLang(confirm_ins_edit_panel.dt_stop, 'en'), datetoStringOfLang(confirm_ins_edit_panel.dt_stop, 'en'));
                    } else {
                        this.confirm_date_start.data('dateRangePicker').setDateRange(datetoStringOfLang(confirm_ins_edit_panel.dt_start, 'ru'), datetoStringOfLang(confirm_ins_edit_panel.dt_start, 'ru'));
                        if (confirm_ins_edit_panel.dt_stop !== null) this.confirm_date_stop.data('dateRangePicker').setDateRange(datetoStringOfLang(confirm_ins_edit_panel.dt_stop, 'ru'), datetoStringOfLang(confirm_ins_edit_panel.dt_stop, 'ru'));

                    }
                    this.confirm_place.val(result_mw !== null ? result_mw.id_place : -1).selectmenu("refresh");
                    this.confirm_site.val(result_mw.site);
                    this.confirm_classification.val(result_mw !== null ? result_mw.id_classification : -1).selectmenu("refresh");
                    this.confirm_nature.val(result_mw !== null ? result_mw.id_nature : -1).selectmenu("refresh");
                    this.confirm_num.val(result_mw.num);
                    this.confirm_cause.val(result_mw !== null ? result_mw.id_cause : -1).selectmenu("refresh");
                    this.confirm_type_cause.val(result_mw !== null ? result_mw.id_type_cause : -1).selectmenu("refresh");
                    this.confirm_subdivision.val(result_mw !== null ? result_mw.id_subdivision : -1).selectmenu("refresh");
                    this.confirm_akt.val(result_mw.akt);
                    this.confirm_locomotive_series.val(result_mw.locomotive_series);
                    this.confirm_driver.val(result_mw.driver);
                    this.confirm_helper.val(result_mw.helper);
                    this.confirm_measures.val(result_mw.measures);
                    this.confirm_note.val(result_mw.note);

                } else {
                    // режим insert
                    confirm_ins_edit_panel.id = 0;
                    confirm_ins_edit_panel.dt_start = null;
                    confirm_ins_edit_panel.dt_stop = null;
                }
            },

            // Сбросить
            resetMarriage: function () {
                $('input#confirm-date-start').text('');
                $('input#confirm-date-stop').text('');
                this.confirm_place.val(-1).selectmenu("refresh");
                this.confirm_site.val('');
                this.confirm_classification.val(-1).selectmenu("refresh");
                this.confirm_nature.val(-1).selectmenu("refresh");
                this.confirm_num.val('');
                this.confirm_cause.val(-1).selectmenu("refresh");
                this.confirm_type_cause.val(-1).selectmenu("refresh");
                this.confirm_subdivision.val(-1).selectmenu("refresh");
                this.confirm_akt.val('');
                this.confirm_locomotive_series.val('');
                this.confirm_driver.val('');
                this.confirm_helper.val('');
                this.confirm_measures.val('');
                this.confirm_note.val('');

            },
            // Получить cards после правки
            getNewMarriageWork: function () {
                var now = new Date();
                return marriage_work = {
                    id: Number(confirm_ins_edit_panel.id),
                    date_start: confirm_ins_edit_panel.dt_start,
                    date_stop: confirm_ins_edit_panel.dt_stop,
                    id_place: confirm_ins_edit_panel.confirm_place.val() !== "-1" ? Number(confirm_ins_edit_panel.confirm_place.val()) : null,
                    site: confirm_ins_edit_panel.confirm_site.val(),
                    id_classification: confirm_ins_edit_panel.confirm_classification.val() !== "-1" ? Number(confirm_ins_edit_panel.confirm_classification.val()) : null,
                    id_nature: confirm_ins_edit_panel.confirm_nature.val() !== "-1" ? Number(confirm_ins_edit_panel.confirm_nature.val()) : null,
                    num: confirm_ins_edit_panel.confirm_num.val(),
                    id_cause: confirm_ins_edit_panel.confirm_cause.val() !== "-1" ? Number(confirm_ins_edit_panel.confirm_cause.val()) : null,
                    id_type_cause: confirm_ins_edit_panel.confirm_type_cause.val() !== "-1" ? Number(confirm_ins_edit_panel.confirm_type_cause.val()) : null,
                    id_subdivision: confirm_ins_edit_panel.confirm_subdivision.val() !== "-1" ? Number(confirm_ins_edit_panel.confirm_subdivision.val()) : null,
                    akt: confirm_ins_edit_panel.confirm_akt.val(),
                    locomotive_series: confirm_ins_edit_panel.confirm_locomotive_series.val(),
                    driver: confirm_ins_edit_panel.confirm_driver.val(),
                    helper: confirm_ins_edit_panel.confirm_helper.val(),
                    measures: confirm_ins_edit_panel.confirm_measures.val(),
                    note: confirm_ins_edit_panel.confirm_note.val(),
                    create: confirm_ins_edit_panel.id > 0 ? confirm_ins_edit_panel.create_date : now,
                    create_user: confirm_ins_edit_panel.id > 0 ? confirm_ins_edit_panel.create_user : confirm_ins_edit_panel.user,
                    change: now,
                    change_user: confirm_ins_edit_panel.user,
                };
            },
        },
        // Панель добавить обновить карту
        confirm_delete_panel = {
            html_div: $("#delete-confirm"),
            id: 0,
            initObject: function () {
                this.obj = this.html_div.dialog({
                    resizable: false,
                    modal: true,
                    autoOpen: false,
                    height: "auto",
                    width: 600,
                    buttons: {
                        "Удалить": function () {
                            $(this).dialog("close");
                            deleteAsynczsCards(confirm_delete_panel.id,
                                function (result) {
                                    // Проверка на ошибку
                                    if (result > 0) {
                                        // ошибки нет
                                        table_report_1.viewDeleteString(confirm_delete_panel.id)

                                    } else {
                                        alert('error delete')
                                    }
                                });
                        },
                        Cancel: function () {
                            $(this).dialog("close");
                        }
                    }
                });
                $('#legend-delete').text(langView('legend_delete_text', langs));
                $('#label-id-cards').text(langView('field_id', langs) + ':');
                $('#label-number-cards').text(langView('field_Number', langs) + ':');
                $('#label-drivername-cards').text(langView('field_DriverName', langs) + ':');
                $('#label-autonumber-cards').text(langView('field_AutoNumber', langs) + ':');
                $('#label-debitor-cards').text(langView('field_Debitor', langs) + ':');
                $('#label-sn1-cards').text(langView('field_Sn1', langs) + ':');
                $('#label-sn2-cards').text(langView('field_Sn2', langs) + ':');
                $('#label-automodel-cards').text(langView('field_AutoModel', langs) + ':');
                $('#label-street-cards').text(langView('field_Street', langs) + ':');
                $('#label-house-cards').text(langView('field_House', langs) + ':');
                $('#label-owner-cards').text(langView('field_Owner', langs) + ':');
                $('#label-active-cards').text(langView('field_Active', langs) + ':');
            },
            Open: function (id) {
                if (id != null) {
                    getAsyncViewazsCardsOfID(id,
                        function (result) {
                            confirm_delete_panel.setCards(result);
                            confirm_delete_panel.obj.dialog("option", "title", langView('title_delete_cards', langs));
                            confirm_delete_panel.obj.dialog("open");
                        });
                }
            },
            setCards: function (cards) {
                if (cards != null) {
                    confirm_delete_panel.id = cards.Id;
                    $('#value-id-cards').text(cards.Id);
                    $('#value-number-cards').text(cards.Number);
                    $('#value-drivername-cards').text(cards.DriverName);
                    $('#value-autonumber-cards').text(cards.AutoNumber);
                    $('#value-debitor-cards').text(cards.Debitor);
                    $('#value-sn1-cards').text(cards.Sn1);
                    $('#value-sn2-cards').text(cards.Sn2);
                    $('#value-automodel-cards').text(cards.AutoModel);
                    $('#value-street-cards').text(cards.Street);
                    $('#value-house-cards').text(reference_departs != null ? reference_departs.getText(cards.House) : cards.House);
                    $('#value-owner-cards').text(cards.Owner);
                    $('#value-active-cards').text(cards.Active);
                }
            },
        }
    //-----------------------------------------------------------------------------------------
    // Функции
    //-----------------------------------------------------------------------------------------

    //-----------------------------------------------------------------------------------------
    // Инициализация объектов
    //-----------------------------------------------------------------------------------------

    confirm_delete_panel.initObject();
    tab_type_report.initObject();
    // Загрузка библиотек
    loadReference(function (result) {
        confirm_ins_edit_panel.initObject();
        table_report_1.initObject();
        tab_type_report.activeTable(tab_type_report.active, true);
    });

});
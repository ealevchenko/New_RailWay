(function ($) {


    // Список общесистемных слов 
    $.KISCommon =
        {
            'default':  //default language: ru
            {
                'mess_delay': 'Мы обрабатываем ваш запрос...',
                'label_view_first': 'Прибытие',
                'label_view_last': 'Отправка',
            },
            'en':  //default language: English
            {
                'mess_delay': 'We are processing your request ...',
                'label_view_first': 'Arrival',
                'label_view_last': 'Sending',
            }

        };

    // Список слов для описания таблиц
    $.KISTable =
        {
            'default':  //default language: ru
            {
                "dt_decimal": "",
                "dt_emptyTable": "Нет данных в таблице",
                "dt_info": "Отображение _START_ по _END_ из _TOTAL_ записей",
                "dt_infoEmpty": "Отображение 0 to 0 of 0 записей",
                "dt_infoFiltered": "(отфильтровано из _MAX_ всего записей)",
                "dt_infoPostFix": "",
                "dt_thousands": ".",
                "dt_lengthMenu": "Показать  _MENU_ записей",
                "dt_loadingRecords": "Загрузка...",
                "dt_processing": "Обработка ...",
                "dt_search": "Найти:",
                "dt_zeroRecords": "Не найдено совпадающих записей",
                "dt_paginate": {
                    "first": "Первая",
                    "last": "Последняя",
                    "next": "Следующая",
                    "previous": "Предыдущая"
                },
                "dt_aria": {
                    "sortAscending": ": активировать сортировку столбца по возрастанию",
                    "sortDescending": ": активировать сортировку колонки по убыванию"
                }

            },
            'en':  //default language: English
            {
                "dt_decimal": "",
                "dt_emptyTable": "No data available in table",
                "dt_info": "Showing _START_ to _END_ of _TOTAL_ entries",
                "dt_infoEmpty": "Showing 0 to 0 of 0 entries",
                "dt_infoFiltered": "(filtered from _MAX_ total entries)",
                "dt_infoPostFix": "",
                "dt_thousands": ",",
                "dt_lengthMenu": "Show _MENU_ entries",
                "dt_loadingRecords": "Loading...",
                "dt_processing": "Processing...",
                "dt_search": "Search:",
                "dt_zeroRecords": "No matching records found",
                "dt_paginate": {
                    "first": "First",
                    "last": "Last",
                    "next": "Next",
                    "previous": "Previous"
                },
                "dt_aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                }

            }

        };

    // Список слов
    $.KISFields =
        {
            'default':  //default language: ru
            {
                'table_field_field': 'Поле',
                'table_field_value': 'Значение',
            },
            'en':  //default language: English
            {
                'table_field_field': 'Field',
                'table_field_value': 'Value',
            }

        };
    // Метод определения списка по указаному языку
    var getLanguages = function (languages, lang) {
        if (lang == 'auto') {
            var language = navigator.language ? navigator.language : navigator.browserLanguage;
            if (!language) return languages['default'];
            var language = language.toLowerCase();
            for (var key in languages) {
                if (language.indexOf(key) != -1) {
                    return languages[key];
                }
            }
            return languages['default'];
        }
        else if (lang && lang in languages) {
            return languages[lang];
        }
        else {
            return languages['default'];
        }
    };
    // Показать текст
    var lang = function (t, langs) {
        var _t = t.toLowerCase();
        var re = (t in langs) ? langs[t] : (_t in langs) ? langs[_t] : null;
        return re;
    };

    var getID = function (obj) {
        return obj.attr("id");
    };
    // Показать сообщение
    var messageDelay = function (message) {
        LockScreen(message);
    };
    // Показать значение поля
    var fieldText = function (field, data_string) {
        if ((field) && field.value != null) {
            var text = field.value.str;
            for (i = 0; i < field.value.val.length; i++) {
                text = text.replace(new RegExp('#' + i, 'g'), data_string[field.value.val[i]]);
            }
            return text;
        }
    }

    // Плагин отобразить вагон по прибытию
    $.fn.kisDetaliField = function (method) {

        var name_plagin = "kisDetaliField";
        var defaults = {
            language: 'auto',                           // локализация
            message_delay: true,                        // Показывать сообщение о задержке
            // таблица
            class_table: "table-transfer-detali",       // класс таблицы
            paging: false,
            ordering: false,
            info: false,
            scrollY: null,
            scrollX: false,
            columns: null,                                // Список названий полей

            // Работа с библиотеками
            //reference_result: null,             // массив Справочник результатов
            //reference_states: null,             // массив Справочник стран СНГ
        };

        //var reference_result = null; // массив Справочник результатов
        //var reference_states = null; // массив Справочник стран СНГ

        var methods = {
            // инициализация
            init: function (params) {
                return this.each(function (i, el) {
                    var $this = $(el),
                        data = $this.data();
                    // Если плагин ещё не проинициализирован
                    if (!data.id) {
                        var table_name = 'table-' + getID($this);
                        $(this).data('id', table_name);
                        var option;
                        // Выполним инициализацию
                        if (!params) option = {};
                        option = $.extend(true,
                            defaults,
                            params,
                            {
                                id_table: table_name,

                            }
                        );
                        $(this).data('option', option);

                        // Справочник лаколизации

                        var langs = $.extend(true, $.extend(true, getLanguages($.KISFields, option.language), getLanguages($.KISCommon, option.language)), getLanguages($.KISTable, option.language));
                        $(this).data('langs', langs);

                        $this.append($('<table class="' + option.class_table + '" id="' + table_name + '" cellpadding="5" cellspacing="0" border="0"></table>'))

                        var obj = initTableListOperation(table_name, option, langs, 0);
                        $(this).data('object', obj);

                        // Если указан вагон показать все вагоны
                        if (option.num_car != null) {
                            methods.viewCar.call($this, option.num_car);
                        }
                        if (option.list_cars != null) {
                            methods.viewCars.call($this, option.list_cars);
                        }
                        //initPanel($this.data());
                        //initEventSelectChild($this.data());

                    }
                });
            },
            // Показать вагон
            viewDetali: function (data_string) {
                return this.each(function (i, el) {
                    var $this = $(el);
                    var data = $this.data()
                    loadReference(data,
                        function (result) {
                            data.data_string = data_string;
                            viewRows(data);
                        });
                });
                return $this;
            },

            destroy: function () {
                return this.each(function () {

                    var $this = $(this),
                        data = $this.data(name_plagin + getID($this));

                    $(window).unbind('.' + name_plagin + getID($this));
                    data.tooltip.remove();
                    $this.removeData(name_plagin + getID($this));
                })
            },
        }

        // немного магии
        if (methods[method]) {
            // если запрашиваемый метод существует, мы его вызываем
            // все параметры, кроме имени метода прийдут в метод
            // this так же перекочует в метод
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            // если первым параметром идет объект, либо совсем пусто
            // выполняем метод init
            return methods.init.apply(this, arguments);
        } else {
            // если ничего не получилось
            $.error('Метод "' + method + '" не найден в плагине jQuery.' + name_plagin);
        };

        // Инициализация таблицы
        function initTableListOperation(id, option, langs) {
            return $('#' + id).DataTable({
                "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                "paging": option.paging,
                "ordering": option.ordering,
                "info": option.info,
                //"autoWidth": false,
                //"select": false,
                //"filter": true,
                "scrollY": option.scrollY,
                "scrollX": option.scrollX,
                language: {
                    "decimal": lang('dt_decimal', langs),
                    "emptyTable": lang('dt_emptyTable', langs),
                    "info": lang('dt_info', langs),
                    "infoEmpty": lang('dt_infoEmpty', langs),
                    "infoFiltered": lang('dt_infoFiltered', langs),
                    "infoPostFix": lang('dt_infoPostFix', langs),
                    "thousands": lang('dt_thousands', langs),
                    "lengthMenu": lang('dt_lengthMenu', langs),
                    "loadingRecords": lang('dt_loadingRecords', langs),
                    "processing": lang('dt_processing', langs),
                    "search": lang('dt_search', langs),
                    "zeroRecords": lang('dt_zeroRecords', langs),
                    "paginate": lang('dt_paginate', langs),
                    "aria": lang('dt_aria', langs),
                },
                jQueryUI: false,
                "createdRow": function (row, data, index) {
                    //$(row).attr('id', type == 0 ? data.id_history : data.ID);
                },
                columns: [
                    { data: "Value", title: lang('table_field_field', langs), width: "100px", orderable: true, searchable: true },
                    { data: "Text", title: lang('table_field_value', langs), width: "400px", orderable: false, searchable: false },
                ],
            });
        };
        // Инициализация панели
        //function initPanel(data) {
        //    if (!data.option.detali) return;
        //    var selectViewOperation = function (e) {
        //        var target = $(e.target);
        //        if (target.is("#view-first-operation-" + data.id)) {
        //            data.option.detali_first = true;
        //        }
        //        if (target.is("#view-last-operation-" + data.id)) {
        //            data.option.detali_first = false;
        //        }
        //        viewRows(data)
        //    };

        //    data.option.html_div_panel = $('<div class="dt-buttons setup-operation" id="property-' + data.id + '"></div>');
        //    //if (data.option.html_div_panel != null) data.option.html_div_panel.empty();
        //    data.option.html_div_panel_select = $('<div class="setup-operation" id="view-select-' + data.id + '"></div>');
        //    //if (data.option.html_div_panel_select != null) data.option.html_div_panel_select.empty();
        //    // Выбор отображения 
        //    data.option.label_view_first = $('<label for="view-first-operation-' + data.id + '">' + lang('label_view_first', data.langs) + '</label>');
        //    data.option.radio_view_first = $('<input type="radio" name="mode-' + data.id + '" id="view-first-operation-' + data.id + '" ' + (data.option.detali_first ? 'checked="checked"' : '') + '>');
        //    data.option.label_view_last = $('<label for="view-last-operation-' + data.id + '">' + lang('label_view_last', data.langs) + '</label>');
        //    data.option.radio_view_last = $('<input type="radio" name="mode-' + data.id + '" id="view-last-operation-' + data.id + '" ' + (!data.option.detali_first ? 'checked="checked"' : '') + '>');

        //    // Настроим панель info
        //    data.option.html_div_panel_select
        //        .append(data.option.label_view_first)
        //        .append(data.option.radio_view_first)
        //        .append(data.option.label_view_last)
        //        .append(data.option.radio_view_last)
        //    data.option.html_div_panel
        //        //.append(this.html_div_panel_info)
        //        .append(data.option.html_div_panel_select);
        //    $('DIV#' + data.id + '_wrapper').prepend(data.option.html_div_panel);
        //    data.option.html_div_panel_select.controlgroup();
        //    // определим событие выбора режима
        //    data.option.radio_view_first.on("change", selectViewOperation);
        //    data.option.radio_view_last.on("change", selectViewOperation);
        //};

        //Получить список строк по указаному вагону
        function loadData(data, data_string, callback) {
            //выводим сообщение
            if (data.option.message_delay) messageDelay(lang('mess_delay', data.langs));
            // Загружаем библиотеку
            loadReference(data, function (result) {
                // Обновим данные
                if (typeof callback === 'function') {
                    callback(data_string);
                }
            });
        };
        function addRows(data) {
            data.object.clear();
            // Определим список названия полей согласно локализации
            //var fields_default = getLanguages(data.option.fields, data.option.language);
            for (var field in data.data_string) {

                var fl = data.option.columns[field];

                if ((fl) && fl.visible) {
                    data.object.row.add({
                        "Value": getLanguages(fl.title, data.option.language),
                        "Text": (fl.value) ? fieldText(fl, data.data_string) : data.data_string[field],
                    });
                }
            }
        };
        // Добавить и показать строки
        function viewRows(data) {
            addRows(data);
            data.object.draw();
            // Закрываем сообщения
            if (data.option.message_delay) LockScreenOff();
        };

        // Загрузка библиотек
        function loadReference(data, callback) {
            var count = 2;
            // Загрузка библиотеки результатов  (metallurgtrans.js)
            if (data.option.reference_result == null) {
                //getReferenceArrivalResult(function (result) {
                //    data.option.reference_result = result;
                count -= 1;
                if (count <= 0) {
                    if (typeof callback === 'function') {
                        callback(data);
                    }
                }
                //})
            } else {
                count -= 1;
            }
            // Загрузка библиотеки Справочник стран СНГ  (reference.js)
            if (data.option.reference_states == null) {
                //getReferenceStates(function (result) {
                //    data.option.reference_states = result;
                count -= 1;
                if (count <= 0) {
                    if (typeof callback === 'function') {
                        callback(data);
                    }
                }
                //})
            } else {
                count -= 1;
            }
            //
            //if (count <= 0) {
            //    if (typeof callback === 'function') {
            //        callback(data);
            //    }
            //}
        };
    };

})(jQuery);
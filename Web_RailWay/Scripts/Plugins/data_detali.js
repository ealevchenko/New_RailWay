(function ($) {

    // Список названий полей
    $.Fields =
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

    // Плагин отобразить вагон по прибытию
    $.fn.DataDetali = function (method) {

        var name_plagin = "DataDetali";
        var defaults = {
            language: 'auto',                               // локализация
            message_delay: true,                            // Показывать сообщение о задержке
            // таблица
            class_table: "table-data-detali",               // класс таблицы
            paging: false,
            ordering: false,
            info: false,
            searching: false,                               // Поиск
            scrollY: null,
            scrollX: false,
            columns: null,                                  // Список названий полей

            // Работа с библиотеками
            //reference_result: null,                       // массив Справочник результатов
            //reference_states: null,                       // массив Справочник стран СНГ
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
                        var langs = $.extend(true, $.extend(true, getLanguages($.Fields, option.language), getLanguages($.Text_Common, option.language)), getLanguages($.Text_Table, option.language));
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
            // Показать детали
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
                "searching": option.searching,
                //"autoWidth": false,
                //"select": false,
                //"filter": true,
                "scrollY": option.scrollY,
                "scrollX": option.scrollX,
                language: language_table(langs),
                jQueryUI: true,
                "createdRow": function (row, data, index) {
                    $('td', row).eq(0).attr("style", "font-weight:bold;");
                    if (data.Style) {

                        $('td', row).eq(1).addClass(data.Style);
                    }
                },
                columns: [
                    { data: "Value", title: lang('table_field_field', langs), width: "100px", orderable: false, searchable: false },
                    { data: "Text", title: lang('table_field_value', langs), width: "400px", orderable: false, searchable: true },
                ],
            });
        };

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
            for (var field in data.data_string) {

                if (data.option.columns == null) {
                    data.object.row.add({
                        "Value": field,
                        "Text": data.data_string[field],
                        "Style": null,
                    });
                } else {
                    var fl = data.option.columns[field];
                    if ((fl) && fl.visible) {
                        data.object.row.add({
                            "Value": getLanguages(fl.title, data.option.language),
                            "Text": (fl.value) ? fieldText(fl, data.data_string) : data.data_string[field],
                            "Style": (fl.class) ? fl.class : null,
                        });
                    }
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
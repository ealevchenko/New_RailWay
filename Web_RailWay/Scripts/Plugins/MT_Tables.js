(function ($) {


    // Список общесистемных слов 
    $.MTCommon =
    {
        'default':  //default language: ru
        {
            'mess_delay': 'Мы обрабатываем ваш запрос...',
        },
        'en':  //default language: English
        {
            'mess_delay': 'We are processing your request ...',
        }

    };

    // Список слов для описания таблиц
    $.MTTable =
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
    $.MTFields =
    {
        'default':  //default language: ru
        {
            'field_id_sostav': 'IDSostav',
            'field_num': '№ вагона',
            'field_composition_index': 'Индекс поезда',
            'field_date_operation': 'Дата и время операции',
            'field_position': 'Позиция',
            'field_operation': 'Операция',
            'field_consignee': 'Грузополучатель',
            'field_cargo': 'Груз',
            'field_station_end': 'Станция назначения',
            'field_weight_cargo': 'Вес груза',
            'field_country': 'Страна',
            'field_train_number': '№ поезда',
            'field_arrival_car': 'Вагон принят',
            'field_arrival_doc': 'Документ принятия',
            'field_user_name': 'Имя пользователя',
        },
        'en':  //default language: English
        {
            'field_id_sostav': 'IDSostav',
            'field_num': '# Car',
            'field_composition_index': 'Train index',
            'field_date_operation': 'Operation date and time',
            'field_position': 'Position',
            'field_operation': 'Operation',
            'field_consignee': 'Consignee',
            'field_cargo': 'Cargo',
            'field_station_end': 'Destination station',
            'field_weight_cargo': 'Cargo weight',
            'field_country': 'Country',
            'field_train_number': 'Train number',
            'field_arrival_car': 'Car accepted',
            'field_arrival_doc': 'Acceptance document',
            'field_user_name': 'Username',

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
        //var defaultLanguage = $.MTLanguages['default'];
        //if (re == null) re = (t in defaultLanguage) ? defaultLanguage[t] : (_t in defaultLanguage) ? defaultLanguage[_t] : '';
        return re;
    }

    var getID = function (obj) {
        return obj.attr("id");
    }
    // Показать сообщение
    var messageDelay = function (message) {
        LockScreen(message);
    }

    // Плагин отобразить вагон по прибытию
    $.fn.mtArrivalCar = function (method) {

        var name_plagin = "mtArrivalCar";
        var defaults = {
            language: 'auto',                   // локализация
            message_delay: true,                // Показывать сообщение о задержке
            // таблица
            id_table: 'table-' + getID(this),   // id таблицы
            class_table: "Display compact",     // класс таблицы
            paging: true,
            ordering: true,
            info: false,
            scrollY: null,
            scrollX: true,
            detali: true,
            num_car: 0,                         // показать строки по номеру вагона
        };
        var option = [];
        var langs = [];
        var obj = null;
        var list_data = [];

        var methods = {
            init: function (params) {
                return this.each(function () {

                    var $this = $(this),
                    data = $this.data(name_plagin + getID($this));
                    // Если плагин ещё не проинициализирован
                    if (!data) {

                        // Выполним инициализацию
                        if (!params) option = {};
                        option = $.extend(true, defaults, params);

                        //langs = getLanguages($.MTLanguages, option.language);
                        langs = $.extend(true, $.extend(true, getLanguages($.MTFields, option.language), getLanguages($.MTCommon, option.language)), getLanguages($.MTTable, option.language));

                        $this.append($('<table class="' + option.class_table + '" id="' + option.id_table + '" cellpadding="0" style="width:100%"></table>'))
                        obj = initTableListOperation(option, 0);
                        // Если указан вагон показать все вагоны
                        if (option.num_car > 0) {
                            loadData(option.num_car,
                                function (result) {
                                    viewRows(obj, result);
                                });
                        }

                        // Поставим отметку и сохраним объект таблица
                        $(this).data(name_plagin + getID($this), {
                            //target: $this,
                            obj_table: obj,
                            option: option,
                            langs: langs

                        });

                    }
                });
            },
            // Показать вагоны
            ViewCar: function (num) {
                var $this = $(this);
                if (loadInit($this)) {
                    loadData(num,
                        function (result) {
                            viewRows(obj, result);
                        });
                }
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
        }
        // Загрузить настройки и состояние объекта
        function loadInit(obj_this) {
            data = obj_this.data(name_plagin + getID(obj_this));
            if (data) {
                obj = data.obj_table;
                option = data.option;
                langs = data.langs;
                return true;
            } return false;
        }
        // Инициализация таблицы
        function initTableListOperation(option) {
            return $('#' + option.id_table).DataTable({
                "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                "paging": option.paging,
                "ordering": option.ordering,
                "info": option.info,
                //"select": false,
                //"filter": true,
                "scrollY": option.scrollY,
                "scrollX": option.scrollX,
                language: {
                    "decimal": lang('dt_decimal', langs),
                    "emptyTable": lang('dt_emptyTable', langs) ,
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
                    "paginate": lang('dt_paginate', langs) ,
                    "aria": lang('dt_aria', langs) ,
                },
                jQueryUI: true,
                "createdRow": function (row, data, index) {
                    //$(row).attr('id', type == 0 ? data.id_history : data.ID);
                },
                columns: [
                    {
                        className: 'details-control',
                        orderable: false,
                        data: null,
                        defaultContent: '',
                        searchable: false,
                        width: "30px",
                        visible: option.detali
                    },
                    { data: "IDSostav", title: lang('field_id_sostav', langs), width: "50px", orderable: false, searchable: true },
                    { data: "Num", title: lang('field_num', langs), width: "50px", orderable: false, searchable: false },
                    { data: "CompositionIndex", title: lang('field_composition_index', langs), width: "100px", orderable: false, searchable: false },
                    { data: "DateOperation", title: lang('field_date_operation', langs), width: "100px", orderable: true, searchable: false },
                    { data: "Position", title: lang('field_position', langs), width: "50px", orderable: false, searchable: false },
                    { data: "Operation", title: lang('field_operation', langs), width: "50px", orderable: false, searchable: false },
                    { data: "Consignee", title: lang('field_consignee', langs), width: "50px", orderable: false, searchable: false },
                    { data: "Cargo", title: lang('field_cargo', langs), width: "200px", orderable: false, searchable: false },
                    { data: "StationEnd", title: lang('field_station_end', langs), width: "150px", orderable: false, searchable: false },
                    { data: "Weight", title: lang('field_weight_cargo', langs), width: "50px", orderable: false, searchable: false },
                    { data: "Country", title: lang('field_country', langs), width: "50px", orderable: false, searchable: false },
                    { data: "TrainNumber", title: lang('field_train_number', langs), width: "50px", orderable: false, searchable: false },
                    { data: "Arrival", title: lang('field_arrival_car', langs), width: "150px", orderable: false, searchable: false },
                    { data: "NumDocArrival", title: lang('field_arrival_doc', langs), width: "150px", orderable: false, searchable: false },
                    { data: "UserName", title: lang('field_user_name', langs), width: "150px", orderable: false, searchable: false },
                ],
            });
        };
        // Получить список строк по указаному вагону
        function loadData(num, callback) {
            if (option.message_delay) messageDelay(lang('mess_delay', langs));
            // Обновим данные
            getAsyncArrivalCars(
                num,
                function (result) {
                    list_data = result;
                    if (typeof callback === 'function') {
                        callback(result);
                    }
                }
            );
        };
        // Добавить строку
        function addRow(obj, row) {
            obj.row.add({
                "ID": row.ID,
                "IDSostav": row.IDSostav,
                "Position": row.Position,
                "Num": row.Num,
                //"Country": this.type_load == 0 ? country.state + '(' + history.CountryCode + ')' : history.CountryCode,
                "Country": row.CountryCode,
                "CountryCode": row.CountryCode,
                "Weight": row.Weight,
                "CargoCode": row.CargoCode,
                //"Cargo": history.Cargo,
                "Cargo": row.Cargo + '(' + row.CargoCode + ')',
                "StationCode": row.StationCode,
                "Station": row.Station,
                "StationEnd": row.Station + '(' + row.StationCode + ')',
                "Consignee": row.Consignee,
                "Operation": row.Operation,
                "CompositionIndex": row.CompositionIndex,
                "DateOperation": row.DateOperation,
                "TrainNumber": row.TrainNumber,
                "NumDocArrival": row.NumDocArrival,
                "Arrival": row.Arrival,
                "ParentID": row.ParentID,
                "UserName": row.UserName,
            });
        };
        // Добавить строки
        function addRows(obj, data) {
            obj.clear();
            for (i = 0; i < data.length; i++) {
                addRow(obj, data[i]);
            }
        };
        // Добавить и показать строки
        function viewRows(obj, data) {
            addRows(obj, data);
            obj.draw();
            if (option.message_delay) LockScreenOff();
        }
    };

})(jQuery);
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
    }
    //-----------------------------------------------------------------------------------------
    // Функции
    //-----------------------------------------------------------------------------------------

    //-----------------------------------------------------------------------------------------
    // Инициализация объектов
    //-----------------------------------------------------------------------------------------
    resurses.initObject("/railway/Scripts/RW/rw.json",
        function () {
            // вызывает метод init
            var s = $('#test-plugin').mtArrivalCar({
                language: 'en',
                //message_delay: false,
            });
            s.mtArrivalCar("ViewCar", 52921079);
            //var s = $('#test-plugin').mtArrivalCar({ id_table: 'table-test-plugin', num_car: 52921079 });
            //var s1 = $('#test-plugin-1').mtArrivalCar({ id_table: 'table-test-plugin-1' }).mtArrivalCar("ViewCar", 52921145);
            //s1.mtArrivalCar("ViewCar", 52921145);



            //s = $('#test-plugin').mtHistoryArrivalCarDetali({ language: 'en' });
            //s.mtHistoryArrivalCarDetali('destroy1');
            //$('#test-plugin').mt_arrival_cars_table();
            //$('#test-plugin').mt_arrival_cars_table('destroy');
            //s.mt_arrival_cars_table('destroy');
        }); // локализация



});
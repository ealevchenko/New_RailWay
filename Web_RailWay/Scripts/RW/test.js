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
    // Страны СНГ
    rw_reference_states = null

    //-----------------------------------------------------------------------------------------
    // Функции
    //-----------------------------------------------------------------------------------------

    //-----------------------------------------------------------------------------------------
    // Инициализация объектов
    //-----------------------------------------------------------------------------------------

    var test = $('#test-plugin-detali').kisDetaliField({

        columns: {
            id : { title: { ru: 'id', en: 'id' }, visible: true , data: id},
            datetime : { title: { ru: 'Дата и время', en: 'DateAndTime' }, visible: true },
            natur: { title: { ru: 'Натурный лист', en: 'natur' }, visible: true },
        },
    });
    test.kisDetaliField("viewDetali",
        data = {
            id: 15,
            natur: 5678
    });
    //// вызывает метод init
            //var s = $('.test').mtArrivalCar({
            //    //language: 'en',
            //    //message_delay: false,
            //    detali: true,
            //    //list_cars: { 'table-test-plugin-1': 52921079, 'table-test-plugin-2': 52921145 }
            //    //num_car: 52921079
            //});
            ////s.mtArrivalCar("viewCar", 52921079);
            //s.mtArrivalCar("viewCars", { 'table-test-plugin-1': 52921079, 'table-test-plugin-2': 52921145 });

    //resurses.initObject("/railway/Scripts/RW/rw.json",
    //    function () {

    //        //getReferenceStates(function (result) {
    //        //    rw_reference_states = result;
    //        //    // вызывает метод init
    //        //    var s = $('.test').mtArrivalCar({
    //        //        reference_states:rw_reference_states,
    //        //    });
    //        //    s.mtArrivalCar("viewCars", { 'table-test-plugin-1': 52921079, 'table-test-plugin-2': 52921145 });
    //        //}); // Справочник стран СНГ


    //        //// вызывает метод init
    //        //var s = $('#test-plugin-1').mtArrivalCar({
    //        //    //language: 'en',
    //        //    //message_delay: false,
    //        //    //default_sort_arrival : false,
    //        //    detali: true,
    //        //    //list_cars: { 'table-test-plugin-1': 52921079, 'table-test-plugin-2': 52921145 }
    //        //    //num_car: 52921079
    //        //});
    //        //// вызывает метод init
    //        var s = $('.test').mtArrivalCar({
    //            //language: 'en',
    //            //message_delay: false,
    //            detali: true,
    //            //list_cars: { 'table-test-plugin-1': 52921079, 'table-test-plugin-2': 52921145 }
    //            //num_car: 52921079
    //        });
    //        //s.mtArrivalCar("viewCar", 52921079);
    //        s.mtArrivalCar("viewCars", { 'table-test-plugin-1': 52921079, 'table-test-plugin-2': 52921145 });
    //        //s.mtArrivalCar("viewCar", 52921079);
    //        //var s = $('#test-plugin').mtArrivalCar({ id_table: 'table-test-plugin', num_car: 52921079 });
    //        //var s1 = $('#test-plugin-1').mtArrivalCar({ id_table: 'table-test-plugin-1' }).mtArrivalCar("ViewCar", 52921145);
    //        //s1.mtArrivalCar("ViewCar", 52921145);



    //        //s = $('#test-plugin').mtHistoryArrivalCarDetali({ language: 'en' });
    //        //s.mtHistoryArrivalCarDetali('destroy1');
    //        //$('#test-plugin').mt_arrival_cars_table();
    //        //$('#test-plugin').mt_arrival_cars_table('destroy');
    //        //s.mt_arrival_cars_table('destroy');
    //    }); // локализация



});
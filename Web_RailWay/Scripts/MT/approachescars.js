$(document).ready(function () {
    //// Обнулим сокеты
    //$.cookie('view-cars', '0');

    myVar = $.cookie('lang');

    //Первая выборка
    OnBegin();
    //$('a.delivered').click(function (evt) {
    //    evt.preventDefault();
    //    $('tr.normal').css('display', 'none');
    //    $('tr.delivered').css('display', 'block');
    //    alert('delivered');
    //});

    //$('a.not-delivered').click(function (evt) {
    //    evt.preventDefault();
    //    $('tr.normal').css('display', 'block');
    //    $('tr.delivered').css('display', 'none');
    //    alert('not-delivered');
    //});

    $.ajax({
        url: '/railway/MT/Approaches/ListGroupDateCars/',
        type: 'GET',
        //data: { date_start: $('#date-start').val(), date_stop: $('#date-stop').val() },
        dataType: 'html',
        success: function (data) {
            selectGroupDateCars(data);
        },
        error: function (x, y, z) {
            LockScreenOff();
            alert(x + '\n' + y + '\n' + z);
        }
    });


});

function selectGroupDateCars(data) {

    var target = $("#report-detali");
    target.empty();
    target.append(data);
    LockScreenOff();

    $('a.show-detali-object').click(function (evt) {
        evt.preventDefault();
        var dis =$(this).siblings('.detali-object').css('display')
        var dv = $(this).siblings('.detali-object');
        $(this).siblings('.detali-object').slideToggle();
        if (dis == 'none') {
            // открываем
            var day = $(this).siblings('.detali-object').attr('day');
            var month = $(this).siblings('.detali-object').attr('month');
            var year = $(this).siblings('.detali-object').attr('year');
            var d_start = new Date(year, month - 1, day);
            var d_stop = new Date(year, month - 1, day, 23, 59, 59);
            OnBegin();
                // Получим вагоны
                $.ajax({
                    url: '/railway/MT/Approaches/ListArrivalCarsOfDate/',
                    type: 'GET',
                    data: { date_start: d_start.toISOString(), date_stop: d_stop.toISOString()},
                    dataType: 'html',
                    success: function (data) {
                        var target = dv;
                        target.empty();
                        target.append(data);
                        LockScreenOff();
                        $('a.' + day + month + year).click(function (evt) {
                            evt.preventDefault();
                            var id = $(this).attr('id')
                            $('tr[cargo="' + day + month + year + id + '"]').slideToggle();
                        });
                    },
                    error: function (x, y, z) {
                        LockScreenOff();
                        alert(x + '\n' + y + '\n' + z);
                    }
                });
        }
    });
}

//function listArrivalCars(data) {
//    var target = $("#group-date");
//    target.empty();
//    target.append(data);
//    LockScreenOff();
//}


function OnBegin() {
            myVar = $.cookie('lang');
            LockScreen(myVar == 'en' ? 'We are processing your request ...' : 'Мы обрабатываем ваш запрос...');
        }

function OnFailure(request, error) {
            //alert("This is the OnFailure Callback:" + error);
            LockScreenOff();
            alert("Ошибка: " + error);
        }

function OnComplete(request, status) {
            //alert("This is the OnComplete Callback: " + status);   
            LockScreenOff();
        }



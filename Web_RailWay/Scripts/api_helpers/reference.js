//-------------------------------------------------------
// Для работы с модулем добавте файл common.js
//-------------------------------------------------------
//---------------------------------------------------------
// States - Страны Ж.Д. СНГ
//---------------------------------------------------------
// Веруть годность по прибытию и отправке
function getAsyncStates(callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/reference/states/all',
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}
// Веруть годность по прибытию и отправке
function getAsyncStatesOfID(id, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/reference/states/id/'+id,
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}
//------------------------------------------------------------------------
// Формирование библиотек
//------------------------------------------------------------------------
function getReferenceStates(callback) {
    var ref = {
        list: [],
        initObject: function () {
                getAsyncStates(function (result)
                {
                    ref.list = result;
                    if (typeof callback === 'function') {
                        callback(ref);
                    }
                });
        },
        getCountry: function (id) {
            var country = getObjects(this.list, 'id', id)
            if (country != null && country.length > 0) {
                return country[0];
            }
        },
    }
    ref.initObject();
}
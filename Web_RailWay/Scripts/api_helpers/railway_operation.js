//-------------------------------------------------------
// Для работы с модулем добавте файл common.js
//-------------------------------------------------------

//---------------------------------------------
// ReferenceCountry
//---------------------------------------------
// Получить список справочника Country
function deleteAsyncSaveCar(id, callback) {
    $.ajax({
        type: 'DELETE',
        url: '/railway/api/rw/operation/cars/delete/'+id,
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

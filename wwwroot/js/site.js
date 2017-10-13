
//called from Home controller

function docGuidClick(docguid)
{
    /*
    $.ajax({ url: '/Result/GetDoc/',
    type:'GET',
    success: function(data) { }, 
    data:{guid: docguid},
    statusCode : {
        404: function(content) { alert('Cannot find resource'); },
        500: function(content) { alert('Internal server error'); }
    }, 
    error: function(req, status, errorObj) {
    }});

    */
    $('#resultDetail').empty();
   
    var url = "/Result/GetDoc?guid=" + docguid;
 
    var obj = $('<object type="application/pdf" style="float:left;width:100%;height:100%" border="2" data="'+url+'"></object>');

    $('#resultDetail').append(obj); 
}


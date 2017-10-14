
//called from Home controller

function docGuidClick(docguid)
{
    $('#divHeader').empty();
    $('#divFooter').empty();
    $('#resultDetail').empty();
   
    var url = "/Result/GetDoc?guid=" + docguid;
 
    var obj = $('<object type="application/pdf" style="float:left;width:100vw;height:100vh" border="2" data="'+url+'"></object>');

    $('#resultDetail').append(obj); 
}


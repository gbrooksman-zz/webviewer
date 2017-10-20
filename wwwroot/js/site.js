
// start Home controller 

function docGuidClick(docguid)
{
    $('#divMain').empty();   
    var url = "/Result/GetDoc?guid=" + docguid; 
    var obj = $('<object type="application/pdf" style="float:left;width:100vw;height:100vh" border="2" data="'+url+'"></object>');
    $('#divMain').append(obj); 
}

function fillSubformats() {
    var formatCode = $('#ddFormat').val();
    $.ajax({
        url: '/Home/GetSubformats',
        type: "GET",
        dataType: "JSON",
        data: { Format: formatCode},
        success: function (subformats) {                              
            $("#ddSubformat").html(''); 
            $.each(subformats, function (i, subformat) 
            {   
                $("#ddSubformat").append(
                    $('<option></option>').val(subformat.code).html(subformat.name));
            });
        }
    });
  }

// end Home controller 
  

  






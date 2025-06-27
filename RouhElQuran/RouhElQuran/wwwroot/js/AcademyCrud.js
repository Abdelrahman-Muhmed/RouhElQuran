
$(document).ready(function () {

    //Create Genaric get Function For handle All forms in App

    Get = (url, formContent, formId) => {
        console.log(url);
        try {
            $.ajax({
                url: url,
                type: 'GET',
                contentType: false,
                processData: false,
                success(res) {
                    $("#" + formContent).html(res);
                    console.log(res);

                    $("#" + formId).modal('show');
                        // Reinitialize MultiSelect for dynamically added content
                document.querySelectorAll('[data-multi-select]').forEach((select) => {
                    if (!select.dataset.initialized) {
                        new MultiSelect(select);
                        select.dataset.initialized = true;
                    }
                });
                },
                erorr() {
                    console.log("somthing error happen");

                }
            })
        }
        catch (ex) {
            console.log("somthing error happen :" + ex);
        }

    }

    submit = (url, formId) => {
        try {

            var form = document.getElementById(formId); 
            if (!form) {
                console.error("Form not found!");
                return;
            }
            var formData = new FormData(form);
            $.ajax({
                url: url,
                type: 'POST',
                data: formData,
                contentType: false,
                processData: false,
                success(res) {
                    console.log(res);
                    $("#" + formId).modal('hide');
                    location.reload();  

                }
            })
        }
        catch (ex) {
            console.log("somthing error happen :" + ex);
        }
    }






})

deleteRow = (url) => {
    try {
        $.ajax({
            url: url,
            type: 'POST',
            success: function () {
                console.log("Course deleted successfully");
                location.reload();  
             
            },
            error: function (err) {
                console.log("Error:", err.responseText);
            }
        })
    }
    catch (ex) {
        console.log("Something went wrong: " + ex);
    }
}


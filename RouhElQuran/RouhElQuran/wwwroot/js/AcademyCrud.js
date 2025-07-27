
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




// Global variables for sorting state
let currentSortColumn = '';
let currentSortDirection = 'asc';

// Loading indicator functions
function showLoadingIndicator() {
    if ($('#loading-indicator').length === 0) {
        $('body').append(`
            <div id="loading-indicator" class="position-fixed top-50 start-50 translate-middle" style="z-index: 9999;">
                <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
            </div>
        `);
    }
}

function hideLoadingIndicator() {
    $('#loading-indicator').remove();
}

function showErrorMessage(message) {
    console.error(message);

    // Create toast container if it doesn't exist
    if ($('.toast-container').length === 0) {
        $('body').append(`
            <div class="toast-container position-fixed top-0 end-0 p-3" style="z-index: 9999;">
            </div>
        `);
    }

    const toastHtml = `
        <div class="toast" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="toast-header">
                <strong class="me-auto text-danger">Error</strong>
                <button type="button" class="btn-close" data-bs-dismiss="toast"></button>
            </div>
            <div class="toast-body">
                ${message}
            </div>
        </div>
    `;

    $('.toast-container').append(toastHtml);
    $('.toast').last().toast('show');
}


function initializeSortableHeaders() {
    console.log('Initializing sortable headers...');

    $('.sortable-header').off('click').on('click', function () {
        const column = $(this).data('column');
        console.log('Clicked column:', column);

        if (!column) return;

        // Determine sort direction
        if (currentSortColumn === column) {
            // Same column clicked - toggle direction
            currentSortDirection = currentSortDirection === 'asc' ? 'desc' : 'asc';
        } else {
            // Different column clicked - reset to ascending
            currentSortColumn = column;
            currentSortDirection = 'asc';
        }
        console.log("sort Dirction " + currentSortDirection);
        // Update sort icons
        updateSortIcons(column, currentSortDirection);


        // Apply sort
        applySorting(column, currentSortDirection);
    });
}

// Update sort icons
function updateSortIcons(activeColumn, direction) {
    // Reset all icons
    $('.sortable-header .sort-icon').removeClass('fa-sort-up fa-sort-down').addClass('fa-sort');

    // Update active column icon
    const activeIcon = $(`.sortable-header[data-column="${activeColumn}"] .sort-icon`);
    activeIcon.removeClass('fa-sort');

    if (direction === 'asc') {
        activeIcon.addClass('fa-sort-up');
    } else {
        activeIcon.addClass('fa-sort-down');
    }
}

// Apply sorting via AJAX
function applySorting(column, direction, IsDesc) {


    showLoadingIndicator();
    const isDesc = direction === 'desc';

    $.ajax({
        url: '/Instructor/InstructorHomeSort',
        type: 'POST',
        data: {
            sortBy: column,
            IsDesc: isDesc,
            page: $(this).data('page'),
            pageSize: $(this).data('page-size') || 10

         },
        success: function (data) {
            console.log('Sort successful');
            hideLoadingIndicator();

            // Replace table content
            $('.TableContainer').html(data);

            // Re-initialize sortable headers for the new content
            initializeSortableHeaders();

            // Restore sort icons
            updateSortIcons(column, direction);
        },
        error: function (xhr, status, error) {
            console.error('Sort failed:', error);
            hideLoadingIndicator();
            showErrorMessage('Failed to sort data. Please try again.');
        }
    });
}

// Generic sort initialization
function initializeGenericSort(entityName) {
    console.log(`Initializing generic sort for: ${entityName}`);

    // Store entity name for potential future use
    window.currentEntity = entityName;

    // Initialize sortable headers
    initializeSortableHeaders();
}

// Initialize when document is ready
$(document).ready(function () {
    console.log('Initializing sort functionality...');
    initializeGenericSort('InstructorCourses');
    initializeSortableHeaders();
});

// Re-initialize after AJAX content loads
$(document).on('DOMContentLoaded', function () {
    initializeSortableHeaders();
});


$(document).on('click', '.pagination .page-link', function (e) {
    e.preventDefault();

    var $parent = $(this).closest('.page-item');
    if ($parent.hasClass('disabled') || $parent.hasClass('active')) return;

    var page = $(this).data('page');
    if (!page || page < 1) return;

    var sortBy = $('#currentSortBy').val() || 'Instructor.Salary';
    var isDesc = $('#currentIsDesc').val() === 'true';
    var pageSize = $('#currentPageSize').val() || 10;

    $.ajax({
        url: '/Instructor/InstructorHomeSort',
        type: 'POST',
        data: {
            sortBy: sortBy,
            IsDesc: isDesc,
            page: page,
            pageSize: pageSize
        },
        success: function (result) {
            console.log(result)
            $('#instructorTableContainer').replaceWith(result);
            initializeSortableHeaders();

        }
    });
});



(function ($) {
    'use strict';
    
    let initUserTable = function (){
        let columns = [
            {
                data: function (e) {
                    return `<span title="${e.fullName}">${e.fullName}</span>`
                },
                width: '150px'
            },
            {
                data: function (e) {
                    return `<span title="${e.email}">${e.email}</span>`
                },
                width: '150px'
            },
            {
                data: function (e) {
                    return `<span title="${e.roles}">${e.roles}</span>`
                },
                width: '150px'
            },
            {
                data: function (e) {
                    let obj = switchStatus(e.status);
                    return `<label title="${obj.text}" class="${obj.className}">${obj.text}</label>`
                },
                width: '50px'
            },
            {
                data: function (e) {
                    return `<img class="custom-img-border code-mega-border datatable-img" src="${e.thumbnail}" alt="thumbnail">`
                },
                width: '50px'
            },
            {
                data: function (e) {
                    return `<a href="/Admin/User/Detail?id=${e.id}">Thông Tin Người Dùng</a>`
                },
                width: '50px    '
            }
        ];

        coreDatatable('user-datatable', '/Admin/User/GetListUser?PageNumber=1&RowOfPage=1000000&Status=0', 'GET', false, true, columns, null);
    }

    let bindingControl = function () {
        addClassActiveNav('nav-user');

    }
    
    //Load functions
    $(document).ready(function () {
        initUserTable();
        bindingControl();
    });

})(jQuery);

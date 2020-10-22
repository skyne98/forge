if (!window.datatableBlazor) {
    window.datatableBlazor = {};
}
window.datatableBlazor = {
    /**
     * Create a new DataTable
     * @param {string} table
     * @param {string} getData
     * @param {string} columns
     * @param {string} detailPageUrl
     * @param {boolean} editButton
     */
    createTable: function (table, dotnetHelper, getData, columns, detailPageUrl, editButton) {
        let columnsParsed = JSON.parse(columns);
        if (editButton) {
            columnsParsed.push({
                data: null,
                bSortable: false,
                render: function (data, type, row) { return '<a class="nav-link" href="' + detailPageUrl + data.id + '">Edit</a>'; }
            });
        }

        console.log('JS -> Creating a table', table, 'with getData', getData, 'and columns', columnsParsed);

        $(document).ready(function () {
            $(table).DataTable({
                "processing": true,
                "serverSide": true,
                "searching": false,
                "paging": true,
                "autoWidth": false,
                "pagingType": "first_last_numbers",
                "dom": '<"top"f>rt<"bottom"lip><"clear">',
                "ajax": function (data, callback, settings) {
                    console.log('JS -> Trying to call getData', getData, 'with params', data);
                    var request = dotnetHelper.invokeMethodAsync(getData, data);
                    return request.then(data => {
                        console.log('JS -> Got data', data);
                        return JSON.parse(data);
                    }).then(callback).catch(console.error);
                },
                "columns": columnsParsed
            });
        });
    },
    removeTable: function (table) {
        $(document).ready(function () {
            $(table).DataTable().destroy(true);

            let wrapperSelector = table + "_wrapper";
            console.log('JS -> Removing table ' + wrapperSelector);
            $(wrapperSelector).remove();
        });
    },
    reloadTable: function (table) {
        $(document).ready(function () {
            console.log('JS -> Reloading the table', table);
            $(table).DataTable().ajax.reload();
        });
    }
};
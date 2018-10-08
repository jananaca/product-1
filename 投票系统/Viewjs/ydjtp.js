$(function () {
    query()
});

function query() {
    $("#list").hiMallDatagrid({
        url: '/vote/bmlist',
        nowrap: false,
        rownumbers: true,
        NoDataMsg: '没有找到符合条件的数据',
        border: false,
        fit: true,
        fitColumns: true,
        pagination: true,
        idField: "Id",
        pageSize: 10,
        pageNumber: 1,
        queryParams: getQuery(),
        toolbar: /*"#goods-datagrid-toolbar",*/'',
        columns:
        [[
            { field: "Id", hidden: true },
            { field: "name", title: '项目名称' },
            { field: "BH", title: '编号' },
            {
                field: "jssp", title: "技术水平",
               
            },
            {
                field: "jsnd", title: "技术难度",
                
            },
              {
                  field: "zscq", title: "知识产权",
                  
              },
              {
                  field: "jjxy", title: "经济效益",
                
              },
                  {
                      field: "shxy", title: "社会效益",
                      
                  },
                  {
                      field: "tgyy", title: "应用推广"
                  },
                   { field: "dfhj", title: '打分合计' },
                    { field: "tjhj", title: '推荐获奖' },
                    {
                        field: "operation", title: "投票",
                        formatter: function (value, row, index) {
                            var id = row.Id.toString();
                            var html = ["<span class=\"btn-a\">"];
                            html.push("<select onchange='change(" + row.Id + ")' id='select" + row.Id + "'>");
                          
                            if (row.state != 1) {
                                html.push("<option value='1'>同意</option>")
                                html.push("<option selected value='2'>反对</option>")
                            }
                            if (row.state != 2) {
                                html.push("<option selected value='1'>同意</option>")
                                html.push("<option value='2'>反对</option>")
                            }
                            html.push("</select>")
                            html.push("</span>");
                            return html.join("");
                        }
                    }

        ]]
    });
}
function getQuery() {
    var result = { state: "进行中" ,jd : 1,lb:7};
    if ($("#name").val() && $("#name").val().length > 0) {
        result.name = $("#name").val();
    }
    if ($("#fzr").val() && $("#fzr").val().length > 0) {
        result.fzr = $("#fzr").val();
    }
    if ($("#category").val() && $("#category").val().length > 0) {
        result.category = $("#category").val();
    }
    if ($("#category1").val() && $("#category1").val().length > 0) {
        result.category1 = $("#category1").val();
    }
    return result;

}
function change(Id) {
    var value = document.getElementById("select"  + Id).value
    $.ajax({
        type: 'POST',
        url: '/vote/change',
        data: { Id: Id,  state: value,lb:7 },
        success: function () { query() },
        dataType: 'json'
    });

}
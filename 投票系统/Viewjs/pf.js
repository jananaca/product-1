$(function () {
    query()
});

function query() {
    $("#list").hiMallDatagrid({
        url: '/score/bmlist',
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
                field: "jssp", title: "技术水平（0-30分）",
                formatter: function (value, row, index) {
                    var id = row.Id.toString();
                    var html = ["<span class=\"btn-a\">"];
                    html.push("<input type='number' value='" + row.jssp + "' max='30' min='0' onblur='change(" + row.Id + ",1)' id='input1" + row.Id + "'/>")
                    html.push("</span>");
                    return html.join("");
                }
            },
            {
                field: "jsnd", title: "技术难度（0-20分）",
                formatter: function (value, row, index) {
                    var id = row.Id.toString();
                    var html = ["<span class=\"btn-a\">"];
                    html.push("<input type='number' value='" + row.jsnd + "' max='20' min='0' onblur='change(" + row.Id + ",2)' id='input2" + row.Id + "'/>")
                    html.push("</span>");
                    return html.join("");
                }
            },
              {
                  field: "zscq", title: "知识产权（0-10分）",
                  formatter: function (value, row, index) {
                      var id = row.Id.toString();
                      var html = ["<span class=\"btn-a\">"];
                      html.push("<input type='number' value='" + row.zscq + "' max='10' min='0' onblur='change(" + row.Id + ",3)' id='input3" + row.Id + "'/>")
                      html.push("</span>");
                      return html.join("");
                  }
              },
              {
                  field: "jjxy", title: "经济效益（0-20分）",
                  formatter: function (value, row, index) {
                      var id = row.Id.toString();
                      var html = ["<span class=\"btn-a\">"];
                      html.push("<input type='number' value='" + row.jjxy + "' max='20' min='0' onblur='change(" + row.Id + ",4)' id='input4" + row.Id + "'/>")
                      html.push("</span>");
                      return html.join("");
                  }
              },
                  {
                      field: "shxy", title: "社会效益（0-10分）",
                      formatter: function (value, row, index) {
                          var id = row.Id.toString();
                          var html = ["<span class=\"btn-a\">"];
                          html.push("<input type='number' value='" + row.shxy + "' max='10' min='0' onblur='change(" + row.Id + ",5)' id='input5" + row.Id + "'/>")
                          html.push("</span>");
                          return html.join("");
                      }
                  },
                  {
                      field: "yytg", title: "应用推广（0-10分）",
                      formatter: function (value, row, index) {
                          var id = row.Id.toString();
                          var html = ["<span class=\"btn-a\">"];
                          html.push("<input type='number' value='" + row.tgyy + "' max='10' min='0' onblur='change(" + row.Id + ",6)' id='input6" + row.Id + "'/>")
                          html.push("</span>");
                          return html.join("");
                      }
                  },
                   { field: "dfhj", title: '打分合计' },
                  
        ]]
    });
}
function getQuery() {
    var result = { state: "进行中" };
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
function change(Id, state)
{
    var value = document.getElementById("input" + state + Id).value
    if (state == 1)
    {
        if (value > 30)
        {
            alert("输入分值错误");
            return 0;
        }

    }
    else if (state == 2)
    {
        if (value > 20)
        {
            alert("输入分值错误");
            return 0;
        }
    }
    else if (state == 3) {
        if (value > 10) {
            alert("输入分值错误");
            return 0;
        }
    }
    else if (state == 4) {
        if (value > 20) {
            alert("输入分值错误");
            return 0;
        }
    }
    else if (state == 5) {
        if (value > 10) {
            alert("输入分值错误");
            return 0;
        }
    }
    else if (state == 6) {
        if (value > 10) {
            alert("输入分值错误");
            return 0;
        }
    }
        $.ajax({
            type: 'POST',
            url: '/score/changepf',
            data: { Id: Id,value:value,state:state },
            success: function () { query() },
            dataType: 'json'
        });
    
}
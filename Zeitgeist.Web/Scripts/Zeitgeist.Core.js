var zg = zg || {};

zg =
{
    AjaxForm: function(idForm, url, s,err) {
        $(idForm).submit(function (e) {
            e.preventDefault();
            $.ajax({
                url: url,
                type: "POST",
                data: $(idForm).serialize(),
                success: s,
                error:err
            });
            return false;
        });
    }

};


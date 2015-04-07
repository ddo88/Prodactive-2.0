var zg = zg || {};

zg =
{
    AjaxForm: function(idForm, url, s) {
        $(idForm).submit(function (e) {
            e.preventDefault();
            $.ajax({
                url: url,
                type: "POST",
                data: $(idForm).serialize(),
                success: s
            });
            return false;
        });
    }

};


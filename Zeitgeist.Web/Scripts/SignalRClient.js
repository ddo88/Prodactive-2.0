
$(function () {
    // Declare a proxy to reference the hub. 
    var chat = $.connection.Chat;

    var notification = $.connection.Notifications;
    // Create a function that the hub can call to broadcast messages.
    chat.client.broadcastMessage = broadcast;

    notification.client.hello = function() {
        //alert("Hola");
    };
    // Get the user name and store it to prepend to messages.
    //$('#displayname').val(prompt('Enter your name:', ''));
    // Set initial focus to message input box.  
    $('#message').focus();
    // Start the connection.
    $.connection.hub.start().done(function () {


    //asocio eventos de click , para envio de mensajes desde de realizar la conexion

        $('#sendmessage').click(sendData);
        $('#message').keypress(function (e) {
            if (e.which == 13) {
                sendData();
            }
        });
    });

    function sendData() {
        chat.server.sendToBroadcast($('#message').val());
        //r.done(function () { alert("mmm"); });
        // Clear text box and reset focus for next comment. 
        $('#message').val('').focus();
    };

});


function broadcast(message){
    //alert(message);
    model.addMessages(message);
};
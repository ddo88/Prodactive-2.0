(function (angular) {
    'use strict';

    // MessageHub depends on the HubProxy
    //MessageHub.$inject = ['HubProxy'];
    function MessageHub(HubProxy) {
        var messageHub = new HubProxy('Chat', { connectionPath: '/signalr', loggingEnabled: true });
        messageHub.start();
        return messageHub; // return MessageHub instance
    };

    function NotificationHub(HubProxy) {
        var messageHub = new HubProxy('Notifications', { connectionPath: '/signalr', loggingEnabled: true });
        messageHub.start();
        return messageHub; // return MessageHub instance
    };

    angular.module('other', ['services.hub'])
            .factory('MessageHub',      MessageHub)
            .factory('NotificationHub', NotificationHub);

})(window.angular);

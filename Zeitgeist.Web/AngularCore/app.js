(function (angular) {
    'use strict';

    // Register the 'app' module.
    // It's bootstrapped by the ng-app directive.
    var app = angular.module('app', ['services.hub', 'other', 'angularMoment']);// Dependency on the SignalR Hub Service module

    app.controller('MessageController', messageController);

    app.controller('NotificationController', NotificationController);

    app.directive('ngEnter', function (){
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if(event.which === 13){
                    scope.$apply(function(){
                        scope.$eval(attrs.ngEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    });


    function messageController(MessageHub) {

        // Internal reference
        var self = this;

        // Message list that we'll manage and expose to the view
        this.messages = [];

        // Message model that we'll bind to in the view
        this.message = '';
        
        // Internal Method that adds the message object to the message list
        function addMessage(message) {
            self.messages.push(message);
        }

        /**
         * Handles the ng-submit event coming from the form view
         */
        this.sendMessage = function() {
            // Add the message to the chat message list
            //addMessage({ from: 'me', txt: this.message });
            // Send the message to the message hub
            //MessageHub.invoke('Otrho');
            MessageHub.invoke('SendToBroadcast', this.message);
            // Clear the message model
            self.message = '';
            return false;
        };

        // Listen to the Hub event for new messages.
        // Notice, we're not calling $apply here.
        // This is handled for us by the MessageHub.
        //
        MessageHub.on('broadcastMessage', function (message) {
            message.time = new Date();
            addMessage(message);
        });

        //MessageHub.on('Otrho', function () {
        //    alert('ajue')
        //});
    };

    function NotificationController(NotificationHub) {

        // Internal reference
        var self = this;

        // Message list that we'll manage and expose to the view
        this.notifications = [];

        this.id = 0;

        // Internal Method that adds the message object to the message list
        function addNotification(notification) {
            self.notifications.push(notification);
        };

        function removeNotification(notification) {
            delete self.notifications[notification];
        };

        /**
         * Handles the ng-submit event coming from the form view
         */
        this.test = function() {
            NotificationHub.invoke('Test');
            return false;
        };

        this.deleteNotification = function () {
            // Add the message to the chat message list
            //addMessage({ from: 'me', txt: this.message });
            // Send the message to the message hub
            //MessageHub.invoke('Otrho');
            NotificationHub.invoke('DeleteNotification', self.id);
            // Clear the message model
            self.id = 0;
            return false;
        };
        
        NotificationHub.on('Notify', function (notification) {
            addNotification(notification);
        });


        NotificationHub.on('RemoveNotification', function (id) {
            removeNotification(id);
        });

    };


})(window.angular);
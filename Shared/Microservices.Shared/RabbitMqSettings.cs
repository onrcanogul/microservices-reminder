namespace Microservices.Shared
{
    public class RabbitMqSettings
    {
        //public const string Order_OrderCreatedEventQueue_PE = "order-order-created-event-queue";
        public const string Payment_Order_PaymentCompletedEventQueue = "payment-order-payment-completed-event-queue";
        public const string Payment_Order_PaymentFailedEventQueue = "payment-order-payment-failed-event-queue";
        public const string Catalog_ProductAvailableMessageQueue = "catalog-product-available-message-queue";
        public const string Catalog_ProductNotAvailableMessageQueue = "catalog-product-not-available-message-queue";
        public const string CatalogOutbox_ProductUpdatedEventQueue = "catalog-outbox-product-updated-event-queue";
        public const string CatalogOutbox_ProductDeletedEventQueue = "catalog-outbox-product-deleted-event-queue";
        public const string OrderOutbox_OrderCreatedInboxEventQueue = "order-outbox-order-created-inbox-event-queue";
        public const string OrderInbox_OrderCreatedInboxEventQueue = "order-inbox-order-created-inbox-event-queue";
        public const string CatalogInbox_ProductUpdatedEventQueue = "catalog-inbox-product-updated-event-queue";
        public const string CatalogInbox_ProductDeletedEventQueue = "catalog-inbox-product-deleted-event-queue";
    }
}

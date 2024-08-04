namespace Microservices.Shared
{
    public class RabbitMqSettings
    {
        public const string Order_OrderCreatedEventQueue_PE = "order-order-created-event-queue";
        public const string Payment_Order_PaymentCompletedEventQueue = "payment-order-payment-completed-event-queue";
        public const string Payment_Order_PaymentFailedEventQueue = "payment-order-payment-failed-event-queue";
        public const string Catalog_ProductAvailableMessageQueue = "catalog-product-available-message-queue";
        public const string Catalog_ProductNotAvailableMessageQueue = "catalog-product-not-available-message-queue";
        public const string Catalog_ProductUpdatedEventQueue = "catalog-product-updated-event-queue";
        public const string Catalog_ProductDeletedEventQueue = "catalog-product-deleted-event-queue";
    }
}

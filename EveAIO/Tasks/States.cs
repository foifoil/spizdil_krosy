namespace EveAIO.Tasks
{
    using EveAIO;
    using EveAIO.Pocos;
    using System;
    using System.Runtime.InteropServices;

    internal static class States
    {
        internal static string GetTaskState(TaskState state)
        {
            switch (state)
            {
                case TaskState.WAITING_FOR_RESTOCK:
                    return "Waiting for restock";

                case TaskState.WAITING_FOR_PRODUCT:
                    return "Waiting for restock";

                case TaskState.CHECKOUT:
                    return "Successfully checked out";

                case TaskState.ADDING_TO_CART:
                    return "Adding to cart";

                case TaskState.CHECKING_STOCK:
                    return "Checking stock availability";

                case TaskState.SEARCHING:
                    return "Searching for products";

                case TaskState.PREPARING_SMART_CHECKOUT:
                    return "Preparing smart checkout";

                case TaskState.SCHEDULED:
                    return "Scheduled";

                case TaskState.LOGGING_IN:
                    return "Logging in";

                case TaskState.STOPPED:
                    return "Stopped";

                case TaskState.ERROR:
                    return "Error occured";

                case TaskState.ERROR_ATC:
                    return "Add to cart error";

                case TaskState.ERROR_STOCKCHECK:
                    return "Stock check error";

                case TaskState.ERROR_API_EXTRACTION:
                    return "Error extracting API key";

                case TaskState.ERROR_SHIPPING:
                    return "Shipping error";

                case TaskState.ERROR_BILLING:
                    return "Billing error";

                case TaskState.ERROR_REGISTRATION:
                    return "Registration error";

                case TaskState.ERROR_PAYMENT:
                    return "Payment error";

                case TaskState.ERROR_SEARCH:
                    return "Search error";

                case TaskState.ERROR_LOGIN:
                    return "Login error";

                case TaskState.ERROR_SETTING_UP:
                    return "Error setting up";

                case TaskState.MULTICART:
                    return "Multicart";

                case TaskState.CHECKING_OUT:
                    return "Checking out";

                case TaskState.SUBMITTING_ORDER:
                    return "Submitting order";

                case TaskState.SUBMITTING_REGISTRATION:
                    return "Submitting registration";

                case TaskState.WAITING_FOR_ORDER:
                    return "Waiting for order";

                case TaskState.SUBMITTING_SHIPPING:
                    return "Submitting shipping";

                case TaskState.SUBMITTING_BILLING:
                    return "Submitting billing";

                case TaskState.WAITING_IN_QUEUE:
                    return "Waiting in queue";

                case TaskState.WAITING_FOR_CATPCHA:
                    return "Waiting for captcha";

                case TaskState.CARD_DECLINED:
                    return "Card declined";

                case TaskState.REGISTRATION_DECLINED:
                    return "Registration declined";

                case TaskState.PROFILE_USED:
                    return "Profile already used";

                case TaskState.PRODUCT_OOS:
                    return "Product OOS";

                case TaskState.SIZE_OOS:
                    return "Size OOS";

                case TaskState.NO_PAYMENT_METHOD_FOUND:
                    return "No payment method found";

                case TaskState.DUPLICATE_ORDER:
                    return "Duplicate order";

                case TaskState.INVALID_CHECKOUT_LINK:
                    return "Invalid link";

                case TaskState.CHECKING_CHECKOUT_LINK:
                    return "Checking checkout link";

                case TaskState.SHIPPING_NOT_AVAILABLE:
                    return "Shipping not available";

                case TaskState.WAITING_FOR_NEXT_STEP:
                    return "Waiting for next step";

                case TaskState.INITIALIZING:
                    return "Initializing";

                case TaskState.CLEANING_CART:
                    return "Clearing cart";

                case TaskState.LOGIN_UNSUCCESSFUL:
                    return "Login unsuccessful";

                case TaskState.ADDING_TO_CART_UNSUCCESSFUL:
                    return "Adding to cart unsuccessful";

                case TaskState.CHECKOUT_UNSUCCESSFUL:
                    return "Checkout unsuccessful";

                case TaskState.IP_BANNED:
                    return "IP banned";

                case TaskState.IN_STORE_ONLY:
                    return "In store only";

                case TaskState.GETTING_COOKIES:
                    return "Getting cookies";

                case TaskState.CHECKING_INVENTORY:
                    return "Checking inventory";

                case TaskState.SESSION_EXPIRED:
                    return "Session expired";

                case TaskState.D3_SECURE_FAILED:
                    return "3D secure failed";

                case TaskState.ERROR_PROCESSING_CC:
                    return "Error processing CC";

                case TaskState.BROWSER_MISSING:
                    return "Browser missing";

                case TaskState.CLOUDFRONT_ERROR:
                    return "Cloudfront error";

                case TaskState.UNSUPPORTED_GATEWAY:
                    return "Unsupported gateway";

                case TaskState.OPENING_PAYPAL:
                    return "Opening PayPal";

                case TaskState.MANUAL_PICKER:
                    return "Images picker";

                case TaskState.SOLVING_CLOUDFLARE:
                    return "Solving cloudflare";

                case TaskState.STARTING:
                    return "Starting";

                case TaskState.INVALID_CREDIT_CARD:
                    return "Invalid credit card";

                case TaskState.LOGIN_REQUIRED:
                    return "Login required";

                case TaskState.PAGE_NOT_FOUND:
                    return "Page not found";

                case TaskState.WAITING_FOR_CHILD_TASKS:
                    return "Child tasks atc";

                case TaskState.SETTING_UP:
                    return "Setting up";

                case TaskState.PRODUCT_NOT_LIVE_YET:
                    return "Product not live yet";

                case TaskState.REGISTRATION_CLOSED:
                    return "Registration closed";

                case TaskState.REGISTERING:
                    return "Registering";

                case TaskState.WAITING_FOR_REGISTRATION:
                    return "Waiting for registration";

                case TaskState.INVALID_SHIPPING_INFO:
                    return "Invalid shipping info";

                case TaskState.INVALID_BILLING_INFO:
                    return "Invalid billing info";

                case TaskState.APPLYING_COUPON_CODE:
                    return "Applying coupon code";

                case TaskState.EXTRACTING_API_KEY:
                    return "Extracting API key";

                case TaskState.D3_SECURE_CARD_CHECK:
                    return "3d secure check";

                case TaskState.ALREADY_REGISTERED:
                    return "Already registered";

                case TaskState.WATCH_TASK:
                    return "Waiting for watching task";

                case TaskState.US_IP_NEEDED:
                    return "US IP needed";

                case TaskState.BLACKLISTED:
                    return "Blacklisted";

                case TaskState.US_SHIPPING_NEEDED:
                    return "US shipping needed";

                case TaskState.SHOPIFY_SMART_SCHEDULE:
                    return "Shopify smart schedule";

                case TaskState.CASH_IN_ADVANCE_NOT_AVAILABLE:
                    return "Cash in advance not available";

                case TaskState.CREDITCART_NOT_AVAILABLE:
                    return "Credit card not available";

                case TaskState.PAYMENT_ERROR:
                    return "Payment error";
            }
            return "";
        }

        internal static void WriteLogger(TaskObject task, LOGGER_STATES state, Exception ex = null, string variant = "", string msg = "")
        {
            switch (state)
            {
                case LOGGER_STATES.WAITING_IN_QUEUE:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Waiting in queue"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Waiting in queue"}");
                    return;

                case LOGGER_STATES.ACQUIRING_PAYMENT_TOKEN:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Acquiring payment token"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Acquiring payment token"}");
                    return;

                case LOGGER_STATES.ERROR_DURING_TASK_INIT:
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error during task init"} - {msg}", ex);
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error during task init - " + ex.Message} - {msg}");
                        return;
                    }
                    Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error during task init"}", ex);
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error during task init - " + ex.Message}");
                    return;

                case LOGGER_STATES.IP_BANNED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"IP banned"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"IP banned"}");
                    return;

                case LOGGER_STATES.TASK_INIT:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Task init - " + task.HomeUrl}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Task init - " + task.HomeUrl}");
                    return;

                case LOGGER_STATES.ATC_SHOPIFY:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Adding to cart [variant: " + variant + "]"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Adding to cart [variant: " + variant + "]"}");
                    return;

                case LOGGER_STATES.PRODUCT_OOS:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Product out of stock"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Product out of stock"}");
                    return;

                case LOGGER_STATES.SIZE_OOS:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Picked size out of stock"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Picked size out of stock"}");
                    return;

                case LOGGER_STATES.CARTING_SUCCESSFUL:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Carting successful"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Carting successful"}");
                    return;

                case LOGGER_STATES.CARTING_UNSUCCESSFUL:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Carting unsuccessful"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Carting unsuccessful"}");
                    return;

                case LOGGER_STATES.ERROR_ATC:
                    if (!string.IsNullOrEmpty(msg))
                    {
                        if (ex == null)
                        {
                            Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error adding product to cart"} - {msg}");
                            Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error adding product to cart"} - {msg}");
                            return;
                        }
                        Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error adding product to cart"} - {msg}", ex);
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error adding product to cart - " + ex.Message} - {msg}");
                        return;
                    }
                    Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error adding product to cart"}", ex);
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error adding product to cart - " + ex.Message}");
                    return;

                case LOGGER_STATES.ERROR_API_EXTRACTION:
                    if (!string.IsNullOrEmpty(msg))
                    {
                        if (ex == null)
                        {
                            Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error extracting API key"} - {msg}");
                            Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error extracting API key"} - {msg}");
                            return;
                        }
                        Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error extracting API key"} - {msg}", ex);
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error extracting API key - " + ex.Message} - {msg}");
                        return;
                    }
                    Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error extracting API key"}", ex);
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error extracting API key - " + ex.Message}");
                    return;

                case LOGGER_STATES.CHECKING_STOCK:
                    if (string.IsNullOrEmpty(msg))
                    {
                        Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Checking stock availability"}");
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Checking stock availability"}");
                        return;
                    }
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Checking stock availability"} - {msg}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Checking stock availability"} - {msg}");
                    return;

                case LOGGER_STATES.SUBMITTING_ORDER:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: Submitting order");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: Submitting order");
                    return;

                case LOGGER_STATES.WAITING_FOR_CAPTCHA:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: Waiting for captcha");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: Waiting for captcha");
                    return;

                case LOGGER_STATES.PROFILE_ALREADY_USED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: Profile already used");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: Profile already used");
                    return;

                case LOGGER_STATES.CARD_DECLINED:
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Card was declined"} - {msg}");
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Card was declined"} - {msg}");
                        return;
                    }
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: Card was declined");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: Card was declined");
                    return;

                case LOGGER_STATES.ERROR_SUBMITTING_ORDER:
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error submitting order"} - {msg}", ex);
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error submitting order - " + ex.Message} - {msg}");
                        return;
                    }
                    Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error submitting order"}", ex);
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error submitting order - " + ex.Message}");
                    return;

                case LOGGER_STATES.ERROR_SUBMITTING_REGISTRATION:
                    if (string.IsNullOrEmpty(msg))
                    {
                        Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error submitting registration"}", ex);
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error submitting registration - " + ex.Message}");
                        return;
                    }
                    Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error submitting registration"} - {msg}", ex);
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error submitting registration - " + ex.Message} - {msg}");
                    return;

                case LOGGER_STATES.CARD_NUMBER_INCORRECT:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Card number is incorrect"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Card number is incorrect"}");
                    return;

                case LOGGER_STATES.MSG:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {msg}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {msg}");
                    return;

                case LOGGER_STATES.CHECKOUT_UNSUCCESSFUL:
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Checkout unsuccessful"} - {msg}");
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Checkout unsuccessful"} - {msg}");
                        return;
                    }
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Checkout unsuccessful"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Checkout unsuccessful"}");
                    return;

                case LOGGER_STATES.SUBMITTING_BILLING:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Submitting billing"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Submitting billing"}");
                    return;

                case LOGGER_STATES.ERROR_SUBMITTING_BILLING:
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error submitting billing"} - {msg}", ex);
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error submitting billing - " + ex.Message} - {msg}");
                        return;
                    }
                    Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error submitting billing"}", ex);
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error submitting billing - " + ex.Message}");
                    return;

                case LOGGER_STATES.CALCULATING_SHIPPING:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Calculating shipping rates"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Calculating shipping rates"}");
                    return;

                case LOGGER_STATES.COUNTRY_NOT_SUPPORTED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Country not supported"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Country not supported"}");
                    return;

                case LOGGER_STATES.ERROR_CALCULATING_SHIPPING:
                    if (string.IsNullOrEmpty(msg))
                    {
                        Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error calculating shipping rates"}", ex);
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error calculating shipping rates - " + ex.Message}");
                        return;
                    }
                    Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error calculating shipping rates"} - {msg}", ex);
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error calculating shipping rates - " + ex.Message} - {msg}");
                    return;

                case LOGGER_STATES.PRICE_CHECK_PASSED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Price check passed"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Price check passed"}");
                    return;

                case LOGGER_STATES.PRICE_CHECK_NOT_PASSED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Price check didn't pass"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Price check didn't pass"}");
                    return;

                case LOGGER_STATES.ERROR_CHECKING_STOCK:
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error checking stock availability"} - {msg}", ex);
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error checking stock availability - " + ex.Message} - {msg}");
                        return;
                    }
                    Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error checking stock availability"}", ex);
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error checking stock availability - " + ex.Message}");
                    return;

                case LOGGER_STATES.ERROR_SEARCHING:
                    if (string.IsNullOrEmpty(msg))
                    {
                        Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error searching for products"}", ex);
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error searching for products - " + ex.Message}");
                        return;
                    }
                    Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error searching for products"} - {msg}", ex);
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error searching for products - " + ex.Message} - {msg}");
                    return;

                case LOGGER_STATES.IN_STORE_ONLY:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"In store only"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"In store only"}");
                    return;

                case LOGGER_STATES.CHECKOUTLINK_RECOGNIZED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Checkout link recognized"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Checkout link recognized"}");
                    return;

                case LOGGER_STATES.ATCLINK_RECOGNIZED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Atc link recognized"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Atc link recognized"}");
                    return;

                case LOGGER_STATES.ATC_SIZE:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Adding to cart [size: " + variant + "]"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Adding to cart [size: " + variant + "]"}");
                    return;

                case LOGGER_STATES.ATC_PID:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Adding to cart [PID: " + variant + "]"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Adding to cart [PID: " + variant + "]"}");
                    return;

                case LOGGER_STATES.ATC:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Adding to cart"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Adding to cart"}");
                    return;

                case LOGGER_STATES.GETTING_COOKIES:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Getting cookies"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Getting cookies"}");
                    return;

                case LOGGER_STATES.ERROR_GETTING_COOKIES:
                    if (string.IsNullOrEmpty(msg))
                    {
                        Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error getting cookies"}", ex);
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error getting cookies - " + ex.Message}");
                        return;
                    }
                    Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error getting cookies"} - {msg}", ex);
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error getting cookies - " + ex.Message} - {msg}");
                    return;

                case LOGGER_STATES.CHECKING_INVENTORY:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Checking inventory"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Checking inventory"}");
                    return;

                case LOGGER_STATES.ERROR_CHECKING_INVENTORY:
                    if (string.IsNullOrEmpty(msg))
                    {
                        Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error checking inventory"}", ex);
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error checking inventory - " + ex.Message}");
                        return;
                    }
                    Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error checking inventory"} - {msg}", ex);
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error checking inventory - " + ex.Message} - {msg}");
                    return;

                case LOGGER_STATES.SUBMITTING_SHIPPING:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Submitting shipping"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Submitting shipping"}");
                    return;

                case LOGGER_STATES.ERROR_SUBMITTING_SHIPPING:
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error submitting shipping"} - {msg}", ex);
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error submitting shipping - " + ex.Message} - {msg}");
                        return;
                    }
                    Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error submitting shipping"}", ex);
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error submitting shipping - " + ex.Message}");
                    return;

                case LOGGER_STATES.ERROR_SETTING_UP:
                    break;

                case LOGGER_STATES.SESSION_EXPIRED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Session expired"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Session expired"}");
                    return;

                case LOGGER_STATES.CRASH_DETECTED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Crash detected, repeating request in "} {msg}s");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Crash detected, repeating request in "} {msg}s");
                    return;

                case LOGGER_STATES.CART_CHECK_PASSED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Cart security check passed"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Cart security check passed"}");
                    return;

                case LOGGER_STATES.CART_CHECK_NOT_PASSED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Cart security check didn't pass"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Cart security check didn't pass"}");
                    return;

                case LOGGER_STATES.D3_SECURE_FAILED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"3D secure failed"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"3D secure failed"}");
                    return;

                case LOGGER_STATES.ERROR_PROCESSING_CC:
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error processing credit card"} - {msg}", ex);
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error processing credit card - " + ex.Message} - {msg}");
                        return;
                    }
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Error processing credit card"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error processing credit card"}");
                    return;

                case LOGGER_STATES.WAITING_FOR_ORDER:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Waiting for order"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Waiting for order"}");
                    return;

                case LOGGER_STATES.WAITING_FOR_REGISTRATION:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Waiting for registration"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Waiting for registration"}");
                    return;

                case LOGGER_STATES.CHECKING_OUT:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Checking out"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Checking out"}");
                    return;

                case LOGGER_STATES.TASK_FINISHED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Task finished"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Task finished"}");
                    return;

                case LOGGER_STATES.LOGGING_IN:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Logging in"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Logging in"}");
                    return;

                case LOGGER_STATES.LOGIN_UNSUCCESSFUL:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Login unsuccessful"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Login unsuccessful"}");
                    return;

                case LOGGER_STATES.LOGIN_SUCCESSFUL:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Login successful"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Login successful"}");
                    return;

                case LOGGER_STATES.CLEARING_CART:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Clearing cart"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Clearing cart"}");
                    return;

                case LOGGER_STATES.ERROR_LOGGING_IN:
                    if (string.IsNullOrEmpty(msg))
                    {
                        Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error logging in"}", ex);
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error logging in - " + ex.Message}");
                        return;
                    }
                    Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error logging in"} - {msg}", ex);
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error logging in - " + ex.Message} - {msg}");
                    return;

                case LOGGER_STATES.BROWSER_MISSING:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Browser window not opened"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Browser window not opened"}");
                    return;

                case LOGGER_STATES.CLOUDFRONT_ERROR:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Cloudfront error"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Cloudfront error"}");
                    return;

                case LOGGER_STATES.SEARCHING_FOR_PRODUCTS:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Searching for products"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Searching for products"}");
                    return;

                case LOGGER_STATES.UNSUPPORTED_GATEWAY:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Unsupported payment processor"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Unsupported payment processor"}");
                    return;

                case LOGGER_STATES.CASH_IN_ADVANCE_AVAILABLE:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Cash in advance payment method available"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Cash in advance payment method available"}");
                    return;

                case LOGGER_STATES.CASH_IN_ADVANCE_NOT_AVAILABLE:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Cash in advance payment method not available"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Cash in advance payment method not available"}");
                    return;

                case LOGGER_STATES.CREDIT_CARD_NOT_AVAILABLE:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Credit card payment method not available"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Credit card payment method not available"}");
                    return;

                case LOGGER_STATES.OPENING_PAYPAL:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Opening PayPal window"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Opening PayPal window"}");
                    return;

                case LOGGER_STATES.MULTICART_CANT_BE_STARTED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Multicart task is started automatically with the parent task"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Multicart task is started automatically with the parent task"}");
                    return;

                case LOGGER_STATES.MANUALPICKER_CANT_STARTED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Task to be started via Supreme images picker"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Task to be started via Supreme images picker"}");
                    return;

                case LOGGER_STATES.SOLVING_CLOUDFLARE:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Solving cloudflare"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Solving cloudflare"}");
                    return;

                case LOGGER_STATES.SHIPPING_NOT_AVAILABLE:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Shipping not available"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Shipping not available"}");
                    return;

                case LOGGER_STATES.INVALID_CREDIT_CARD:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Invalid credit card"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Invalid credit card"}");
                    return;

                case LOGGER_STATES.NO_PRODUCTS_FOUND:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"No products on stock found matching the search criteria"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"No products on stock found matching the search criteria"}");
                    return;

                case LOGGER_STATES.TASK_PROCESS_ERROR:
                    Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Error during task processing"}", ex);
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Error during task processing - " + ex.Message}");
                    return;

                case LOGGER_STATES.CANT_BE_SHIPPED:
                    Global.Logger.Error($"['{task.Name} - {task.Guid}']: {"Some items can not be shipped to your chosen destination"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Some items can not be shipped to your chosen destination"}");
                    return;

                case LOGGER_STATES.LOGIN_REQUIRED:
                    if (string.IsNullOrEmpty(msg))
                    {
                        Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Login required"}");
                        Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Login required"}");
                        return;
                    }
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Login required"} - {msg}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Login required"} - {msg}");
                    return;

                case LOGGER_STATES.SCHEDULLING:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Product not live yet. Schedulling start to"} {msg}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Product not live yet. Schedulling start to"} {msg}");
                    return;

                case LOGGER_STATES.PAGE_NOT_FOUND:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Requested webpage not foud (404)"} {msg}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Requested webpage not foud (404)"} {msg}");
                    return;

                case LOGGER_STATES.WAITING_FOR_CHILD_TASKS:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Waiting for child tasks"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Waiting for child tasks"}");
                    return;

                case LOGGER_STATES.SUCCESSFULY_CHECKED_OUT:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Successfuly checked out"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Successfuly checked out"}");
                    return;

                case LOGGER_STATES.SETTING_UP:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Setting up"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Setting up"}");
                    return;

                case LOGGER_STATES.PRODUCT_NOT_LIVE_YET:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Product not live yet"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Product not live yet"}");
                    return;

                case LOGGER_STATES.REGISTRATION_CLOSED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Registration closed"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Registration closed"}");
                    return;

                case LOGGER_STATES.REGISTERING:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Registering"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Registering"}");
                    return;

                case LOGGER_STATES.SUBMITTING_REGISTRATION:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Submitting registration"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Submitting registration"}");
                    return;

                case LOGGER_STATES.REGISTRATION_DECLINED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Registration declined"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Registration declined"}");
                    return;

                case LOGGER_STATES.SUCCESSFULLY_REGISTERED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Successfully registered"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Successfully registered"}");
                    return;

                case LOGGER_STATES.INVALID_SHIPPING_INFO:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Invalid shipping info"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Invalid shipping info"}");
                    return;

                case LOGGER_STATES.INVALID_BILLING_INFO:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Invalid billing info"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Invalid billing info"}");
                    return;

                case LOGGER_STATES.NEGATIVE_KWS_MATCH:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Negative keywords match found"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Negative keywords match found"}");
                    return;

                case LOGGER_STATES.APPLYING_COUPON_CODE:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Applying coupon code"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Applying coupon code"}");
                    return;

                case LOGGER_STATES.COUPON_APPLIED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Coupon successfuly applied"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Coupon successfuly applied"}");
                    return;

                case LOGGER_STATES.COUPON_NOT_APPLIED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Applying coupon code failed"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Applying coupon code failed"}");
                    return;

                case LOGGER_STATES.EXTRACTING_API_KEY:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Extracting API key"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Extracting API key"}");
                    return;

                case LOGGER_STATES.D3_SECURE_CARD_CHECK:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Processing 3d secure card check"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Processing 3d secure card check"}");
                    return;

                case LOGGER_STATES.ALREADY_REGISTERED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Already registered"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Already registered"}");
                    return;

                case LOGGER_STATES.WAITING_FOR_WATCHING_TASK:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Waiting for watcher task"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Waiting for watcher task"}");
                    return;

                case LOGGER_STATES.WOKEN_UP:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Woken up by watching task"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Woken up by watching task"}");
                    return;

                case LOGGER_STATES.US_IP_NEEDED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"US IP needed"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"US IP needed"}");
                    return;

                case LOGGER_STATES.BLACKLISTED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Blacklisted"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Blacklisted"}");
                    return;

                case LOGGER_STATES.US_SHIPPING_NEEDED:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"US shipping needed"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"US shipping needed"}");
                    return;

                case LOGGER_STATES.SHOPIFY_SMART_SCHEDULE:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Shopify smart schedule"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Shopify smart schedule"}");
                    return;

                case LOGGER_STATES.PAYMENT_ERROR:
                    Global.Logger.Info($"['{task.Name} - {task.Guid}']: {"Payment error"}");
                    Helpers.WriteLog($"['{task.Name} - {task.Guid}']: {"Payment error"}");
                    break;

                default:
                    return;
            }
        }

        internal enum LOGGER_STATES
        {
            WAITING_IN_QUEUE,
            ACQUIRING_PAYMENT_TOKEN,
            ERROR_DURING_TASK_INIT,
            IP_BANNED,
            TASK_INIT,
            ATC_SHOPIFY,
            PRODUCT_OOS,
            SIZE_OOS,
            CARTING_SUCCESSFUL,
            CARTING_UNSUCCESSFUL,
            ERROR_ATC,
            ERROR_API_EXTRACTION,
            CHECKING_STOCK,
            SUBMITTING_ORDER,
            WAITING_FOR_CAPTCHA,
            PROFILE_ALREADY_USED,
            CARD_DECLINED,
            ERROR_SUBMITTING_ORDER,
            ERROR_SUBMITTING_REGISTRATION,
            CARD_NUMBER_INCORRECT,
            MSG,
            CHECKOUT_UNSUCCESSFUL,
            SUBMITTING_BILLING,
            ERROR_SUBMITTING_BILLING,
            CALCULATING_SHIPPING,
            COUNTRY_NOT_SUPPORTED,
            ERROR_CALCULATING_SHIPPING,
            PRICE_CHECK_PASSED,
            PRICE_CHECK_NOT_PASSED,
            ERROR_CHECKING_STOCK,
            ERROR_SEARCHING,
            IN_STORE_ONLY,
            CHECKOUTLINK_RECOGNIZED,
            ATCLINK_RECOGNIZED,
            ATC_SIZE,
            ATC_PID,
            ATC,
            GETTING_COOKIES,
            ERROR_GETTING_COOKIES,
            CHECKING_INVENTORY,
            ERROR_CHECKING_INVENTORY,
            SUBMITTING_SHIPPING,
            ERROR_SUBMITTING_SHIPPING,
            ERROR_SETTING_UP,
            SESSION_EXPIRED,
            CRASH_DETECTED,
            CART_CHECK_PASSED,
            CART_CHECK_NOT_PASSED,
            D3_SECURE_FAILED,
            ERROR_PROCESSING_CC,
            WAITING_FOR_ORDER,
            WAITING_FOR_REGISTRATION,
            CHECKING_OUT,
            TASK_FINISHED,
            LOGGING_IN,
            LOGIN_UNSUCCESSFUL,
            LOGIN_SUCCESSFUL,
            CLEARING_CART,
            ERROR_LOGGING_IN,
            BROWSER_MISSING,
            CLOUDFRONT_ERROR,
            SEARCHING_FOR_PRODUCTS,
            UNSUPPORTED_GATEWAY,
            CASH_IN_ADVANCE_AVAILABLE,
            CASH_IN_ADVANCE_NOT_AVAILABLE,
            CREDIT_CARD_NOT_AVAILABLE,
            OPENING_PAYPAL,
            MULTICART_CANT_BE_STARTED,
            MANUALPICKER_CANT_STARTED,
            SOLVING_CLOUDFLARE,
            SHIPPING_NOT_AVAILABLE,
            INVALID_CREDIT_CARD,
            NO_PRODUCTS_FOUND,
            TASK_PROCESS_ERROR,
            CANT_BE_SHIPPED,
            LOGIN_REQUIRED,
            SCHEDULLING,
            PAGE_NOT_FOUND,
            WAITING_FOR_CHILD_TASKS,
            SUCCESSFULY_CHECKED_OUT,
            SETTING_UP,
            PRODUCT_NOT_LIVE_YET,
            REGISTRATION_CLOSED,
            REGISTERING,
            SUBMITTING_REGISTRATION,
            REGISTRATION_DECLINED,
            SUCCESSFULLY_REGISTERED,
            INVALID_SHIPPING_INFO,
            INVALID_BILLING_INFO,
            NEGATIVE_KWS_MATCH,
            APPLYING_COUPON_CODE,
            COUPON_APPLIED,
            COUPON_NOT_APPLIED,
            EXTRACTING_API_KEY,
            D3_SECURE_CARD_CHECK,
            ALREADY_REGISTERED,
            WAITING_FOR_WATCHING_TASK,
            WOKEN_UP,
            US_IP_NEEDED,
            BLACKLISTED,
            US_SHIPPING_NEEDED,
            SHOPIFY_SMART_SCHEDULE,
            PAYMENT_ERROR
        }

        internal enum TaskState
        {
            WAITING_FOR_RESTOCK,
            WAITING_FOR_PRODUCT,
            CHECKOUT,
            ADDING_TO_CART,
            CHECKING_STOCK,
            SEARCHING,
            PREPARING_SMART_CHECKOUT,
            SCHEDULED,
            LOGGING_IN,
            STOPPED,
            ERROR,
            ERROR_ATC,
            ERROR_STOCKCHECK,
            ERROR_API_EXTRACTION,
            ERROR_SHIPPING,
            ERROR_BILLING,
            ERROR_REGISTRATION,
            ERROR_PAYMENT,
            ERROR_SEARCH,
            ERROR_LOGIN,
            ERROR_SETTING_UP,
            MULTICART,
            CHECKING_OUT,
            SUBMITTING_ORDER,
            SUBMITTING_REGISTRATION,
            WAITING_FOR_ORDER,
            SUBMITTING_SHIPPING,
            SUBMITTING_BILLING,
            WAITING_IN_QUEUE,
            WAITING_FOR_CATPCHA,
            CARD_DECLINED,
            REGISTRATION_DECLINED,
            PROFILE_USED,
            PRODUCT_OOS,
            SIZE_OOS,
            NO_PAYMENT_METHOD_FOUND,
            DUPLICATE_ORDER,
            INVALID_CHECKOUT_LINK,
            CHECKING_CHECKOUT_LINK,
            SHIPPING_NOT_AVAILABLE,
            WAITING_FOR_NEXT_STEP,
            INITIALIZING,
            CLEANING_CART,
            LOGIN_UNSUCCESSFUL,
            ADDING_TO_CART_UNSUCCESSFUL,
            CHECKOUT_UNSUCCESSFUL,
            IP_BANNED,
            IN_STORE_ONLY,
            GETTING_COOKIES,
            CHECKING_INVENTORY,
            SESSION_EXPIRED,
            D3_SECURE_FAILED,
            ERROR_PROCESSING_CC,
            BROWSER_MISSING,
            CLOUDFRONT_ERROR,
            UNSUPPORTED_GATEWAY,
            OPENING_PAYPAL,
            MANUAL_PICKER,
            SOLVING_CLOUDFLARE,
            STARTING,
            INVALID_CREDIT_CARD,
            LOGIN_REQUIRED,
            PAGE_NOT_FOUND,
            WAITING_FOR_CHILD_TASKS,
            SETTING_UP,
            PRODUCT_NOT_LIVE_YET,
            REGISTRATION_CLOSED,
            REGISTERING,
            WAITING_FOR_REGISTRATION,
            INVALID_SHIPPING_INFO,
            INVALID_BILLING_INFO,
            APPLYING_COUPON_CODE,
            EXTRACTING_API_KEY,
            D3_SECURE_CARD_CHECK,
            ALREADY_REGISTERED,
            WATCH_TASK,
            US_IP_NEEDED,
            BLACKLISTED,
            US_SHIPPING_NEEDED,
            SHOPIFY_SMART_SCHEDULE,
            CASH_IN_ADVANCE_NOT_AVAILABLE,
            CREDITCART_NOT_AVAILABLE,
            PAYMENT_ERROR
        }
    }
}


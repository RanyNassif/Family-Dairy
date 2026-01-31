(() => {
    const translations = {
        en: {
            "app.name": "Dairy Family Manager",
            "app.tagline": "Daily sales • profits • costs",
            "app.footer": "© {year} Dairy Family Manager",
            "nav.home": "Home",
            "nav.dailyEntry": "Daily Entry",
            "nav.dailySummary": "Daily Summary",
            "nav.setup": "Setup",
            "nav.products": "Products",
            "nav.clients": "Clients",
            "nav.monthlyCosts": "Monthly Costs",
            "nav.reports": "Reports",
            "nav.monthlyDashboard": "Monthly Dashboard",
            "action.logout": "Logout",
            "action.login": "Login",
            "action.addProduct": "Add Product",
            "action.addClient": "Add Client",
            "action.edit": "Edit",
            "action.prices": "Prices",
            "action.save": "Save",
            "action.cancel": "Cancel",
            "action.load": "Load",
            "action.addLine": "Add line",
            "action.remove": "Remove",
            "action.savePrices": "Save Prices",
            "action.back": "Back",
            "action.newDailyEntry": "New Daily Entry",
            "action.viewSummary": "View Summary",
            "action.toggleLang": "English / عربي",
            "action.update": "Update",
            "action.searchProduct": "Search product...",
            "action.searchClient": "Search client...",
            "label.username": "Username",
            "label.password": "Password",
            "label.date": "Date",
            "label.client": "Client",
            "label.product": "Product",
            "label.qty": "Qty",
            "label.unitPrice": "Unit Price",
            "label.saveDefaultPrice": "Save price as default",
            "label.remember": "Remember",
            "label.totalSales": "Total Sales",
            "label.factoryProfit": "Factory Profit",
            "label.distributorProfit": "Distributor Profit",
            "label.distributorNet": "Distributor Net",
            "label.distributorNetAfterBenzine": "Distributor Net (after benzine)",
            "label.benzine": "Benzine (Distributor daily expense)",
            "label.basePrice": "Base Price",
            "label.factoryProfitType": "Factory Profit Type",
            "label.factoryProfitValue": "Factory Profit Value",
            "label.distributorProfitType": "Distributor Profit Type",
            "label.distributorProfitValue": "Distributor Profit Value",
            "label.active": "Active",
            "label.inactive": "Inactive",
            "label.yes": "Yes",
            "label.no": "No",
            "label.distributor": "Distributor",
            "label.deliveredByDistributor": "Delivered by distributor",
            "label.year": "Year",
            "label.month": "Month",
            "label.monthRange": "Month (1-12)",
            "label.monthHelp": "1 to 12",
            "label.factoryNetAfterCosts": "Factory Net After Costs",
            "label.factoryNetAfterCostsShort": "Factory Net After Costs",
            "label.productEn": "Product (EN)",
            "label.productAr": "Product (AR)",
            "label.clientEn": "Client (EN)",
            "label.clientAr": "Client (AR)",
            "label.basePriceShort": "Base Price",
            "label.customPrice": "Custom Price (leave empty to use base)",
            "label.milk": "Milk",
            "label.workers": "Workers",
            "label.gas": "Gas",
            "label.other": "Other",
            "label.nameEn": "Name (EN)",
            "label.nameAr": "Name (AR)",
            "label.labelEn": "Label (EN) (optional)",
            "label.labelAr": "Label (AR) (optional)",
            "label.factoryProfitTypeFixed": "Fixed",
            "label.factoryProfitTypePercent": "Percent",
            "label.lines": "Lines",
            "label.linesSubtitle": "Add the products sold for the selected client.",
            "label.tipSavePrice": "Tip: use “Save price as default” to store client-specific default prices.",
            "label.salesPerDay": "Sales per day",
            "label.topProductsBySales": "Top products by sales",
            "label.factoryProfitPerDay": "Factory profit per day",
            "label.distributorNetPerDay": "Distributor net per day",
            "label.topProductsBySalesTable": "Top Products (by Sales)",
            "label.rankBySales": "Ranked by total sales amount.",
            "label.quantity": "Qty",
            "label.sales": "Sales",
            "label.signIn": "Sign in",
            "label.signInHelp": "Use your username and password to access the system.",
            "label.toggleMenu": "Toggle menu",
            "label.actions": "Actions",
            "label.welcome": "Welcome,",
            "label.homeSubtitle": "Quick access to daily entry, summaries, and admin setup.",
            "label.dailyEntrySubtitle": "Pick a date and client, then add sold products and prices.",
            "label.dailySummarySubtitle": "Totals and breakdowns for the selected date.",
            "label.productsSubtitle": "Define your products, base prices, and profit rules.",
            "label.productsCreateSubtitle": "Define product names, base price, and profit rules.",
            "label.clientsSubtitle": "Manage your clients and their pricing behavior.",
            "label.clientsCreateSubtitle": "Create a new client and set distributor behavior.",
            "label.monthlyCostsTitle": "Monthly Costs",
            "label.monthlyCostsSubtitle": "Track monthly expenses by product.",
            "label.monthlyDashboardSubtitle": "KPIs, charts, and top products for the selected month.",
            "label.savePricesHelp": "Update custom prices for this client.",
            "label.clientPricesTitle": "Client Prices",
            "label.clientNotFound": "Client not found.",
            "label.selectClient": "-- Select client --",
            "label.selectProduct": "-- Select product --",
            "label.searchProductsTitle": "Search product...",
            "label.searchClientsTitle": "Search client...",
            "label.dataEntry": "Data Entry",
            "label.admin": "Admin",
            "label.loadFilters": "Load",
            "label.byClient": "By Client",
            "label.byClientSubtitle": "Sales and profits per client.",
            "label.byProduct": "By Product",
            "label.byProductSubtitle": "Quantity, sales, and profits per product.",
            "label.saveBenzineHelp": "This will be used to compute distributor net for the day.",
            "label.quickDailyEntry": "Daily Sales Entry",
            "label.quickDailyEntrySub": "Add today’s sales lines for a client",
            "label.quickSummary": "Daily Summary",
            "label.quickSummarySub": "Review totals, profits, and distributions",
            "label.quickMonthlyDashboard": "Monthly Dashboard",
            "label.quickMonthlyDashboardSub": "Charts + top products + net results",
            "label.quickProducts": "Products",
            "label.quickProductsSub": "Manage pricing rules & profits",
            "label.quickClients": "Clients",
            "label.quickClientsSub": "Manage clients & default prices",
            "label.quickMonthlyCosts": "Monthly Costs",
            "label.quickMonthlyCostsSub": "Track expenses like benzine and others"
            ,
            "label.editProduct": "Edit Product",
            "label.editClient": "Edit Client"
        },
        ar: {
            "app.name": "مدير ألبان العائلة",
            "app.tagline": "مبيعات يومية • أرباح • تكاليف",
            "app.footer": "© {year} مدير ألبان العائلة",
            "nav.home": "الرئيسية",
            "nav.dailyEntry": "إدخال يومي",
            "nav.dailySummary": "ملخص يومي",
            "nav.setup": "الإعدادات",
            "nav.products": "المنتجات",
            "nav.clients": "العملاء",
            "nav.monthlyCosts": "التكاليف الشهرية",
            "nav.reports": "التقارير",
            "nav.monthlyDashboard": "لوحة الشهر",
            "action.logout": "تسجيل الخروج",
            "action.login": "تسجيل الدخول",
            "action.addProduct": "إضافة منتج",
            "action.addClient": "إضافة عميل",
            "action.edit": "تعديل",
            "action.prices": "الأسعار",
            "action.save": "حفظ",
            "action.cancel": "إلغاء",
            "action.load": "تحميل",
            "action.addLine": "إضافة سطر",
            "action.remove": "حذف",
            "action.savePrices": "حفظ الأسعار",
            "action.back": "رجوع",
            "action.newDailyEntry": "إدخال يومي جديد",
            "action.viewSummary": "عرض الملخص",
            "action.toggleLang": "عربي / English",
            "action.update": "تحديث",
            "action.searchProduct": "ابحث عن منتج...",
            "action.searchClient": "ابحث عن عميل...",
            "label.username": "اسم المستخدم",
            "label.password": "كلمة المرور",
            "label.date": "التاريخ",
            "label.client": "العميل",
            "label.product": "المنتج",
            "label.qty": "الكمية",
            "label.unitPrice": "سعر الوحدة",
            "label.saveDefaultPrice": "احفظ السعر كافتراضي",
            "label.remember": "تذكّر",
            "label.totalSales": "إجمالي المبيعات",
            "label.factoryProfit": "ربح المصنع",
            "label.distributorProfit": "ربح الموزّع",
            "label.distributorNet": "صافي الموزّع",
            "label.distributorNetAfterBenzine": "صافي الموزّع (بعد البنزين)",
            "label.benzine": "بنزين (مصروف يومي للموزّع)",
            "label.basePrice": "السعر الأساسي",
            "label.factoryProfitType": "نوع ربح المصنع",
            "label.factoryProfitValue": "قيمة ربح المصنع",
            "label.distributorProfitType": "نوع ربح الموزّع",
            "label.distributorProfitValue": "قيمة ربح الموزّع",
            "label.active": "نشط",
            "label.inactive": "غير نشط",
            "label.yes": "نعم",
            "label.no": "لا",
            "label.distributor": "الموزّع",
            "label.deliveredByDistributor": "يتم التوصيل عبر الموزّع",
            "label.year": "السنة",
            "label.month": "الشهر",
            "label.monthRange": "الشهر (1-12)",
            "label.monthHelp": "من 1 إلى 12",
            "label.factoryNetAfterCosts": "صافي المصنع بعد التكاليف",
            "label.factoryNetAfterCostsShort": "صافي المصنع بعد التكاليف",
            "label.productEn": "المنتج (إنجليزي)",
            "label.productAr": "المنتج (عربي)",
            "label.clientEn": "العميل (إنجليزي)",
            "label.clientAr": "العميل (عربي)",
            "label.basePriceShort": "السعر الأساسي",
            "label.customPrice": "سعر مخصص (اتركه فارغًا لاستخدام الأساسي)",
            "label.milk": "الحليب",
            "label.workers": "العمال",
            "label.gas": "الغاز",
            "label.other": "أخرى",
            "label.nameEn": "الاسم (إنجليزي)",
            "label.nameAr": "الاسم (عربي)",
            "label.labelEn": "التسمية (إنجليزي) (اختياري)",
            "label.labelAr": "التسمية (عربي) (اختياري)",
            "label.factoryProfitTypeFixed": "ثابت",
            "label.factoryProfitTypePercent": "نسبة",
            "label.lines": "الأسطر",
            "label.linesSubtitle": "أضف المنتجات المباعة للعميل المحدد.",
            "label.tipSavePrice": "معلومة: استخدم “احفظ السعر كافتراضي” لتخزين أسعار مخصصة للعميل.",
            "label.salesPerDay": "المبيعات لكل يوم",
            "label.topProductsBySales": "أعلى المنتجات بالمبيعات",
            "label.factoryProfitPerDay": "ربح المصنع لكل يوم",
            "label.distributorNetPerDay": "صافي الموزّع لكل يوم",
            "label.topProductsBySalesTable": "أعلى المنتجات (حسب المبيعات)",
            "label.rankBySales": "مرتبة حسب إجمالي المبيعات.",
            "label.quantity": "الكمية",
            "label.sales": "المبيعات",
            "label.signIn": "تسجيل الدخول",
            "label.signInHelp": "استخدم اسم المستخدم وكلمة المرور للوصول للنظام.",
            "label.toggleMenu": "تبديل القائمة",
            "label.actions": "الإجراءات",
            "label.welcome": "مرحبًا،",
            "label.homeSubtitle": "وصول سريع للإدخال اليومي والملخصات وإعدادات الإدارة.",
            "label.dailyEntrySubtitle": "اختر التاريخ والعميل ثم أضف المنتجات المباعة والأسعار.",
            "label.dailySummarySubtitle": "الإجماليات والتفاصيل حسب التاريخ المحدد.",
            "label.productsSubtitle": "عرّف المنتجات والأسعار الأساسية وقواعد الربح.",
            "label.productsCreateSubtitle": "عرّف أسماء المنتجات والسعر الأساسي وقواعد الربح.",
            "label.clientsSubtitle": "إدارة العملاء وسلوك التسعير.",
            "label.clientsCreateSubtitle": "أنشئ عميلًا جديدًا وحدد سلوك الموزّع.",
            "label.monthlyCostsTitle": "التكاليف الشهرية",
            "label.monthlyCostsSubtitle": "تتبع المصاريف الشهرية حسب المنتج.",
            "label.monthlyDashboardSubtitle": "مؤشرات ولوحات ورسوم للمنتجات الأعلى في الشهر.",
            "label.savePricesHelp": "حدّث الأسعار المخصصة لهذا العميل.",
            "label.clientPricesTitle": "أسعار العميل",
            "label.clientNotFound": "لم يتم العثور على العميل.",
            "label.selectClient": "-- اختر عميلًا --",
            "label.selectProduct": "-- اختر منتجًا --",
            "label.searchProductsTitle": "ابحث عن منتج...",
            "label.searchClientsTitle": "ابحث عن عميل...",
            "label.dataEntry": "إدخال بيانات",
            "label.admin": "مسؤول",
            "label.loadFilters": "تحميل",
            "label.byClient": "حسب العميل",
            "label.byClientSubtitle": "المبيعات والأرباح حسب العميل.",
            "label.byProduct": "حسب المنتج",
            "label.byProductSubtitle": "الكمية والمبيعات والأرباح حسب المنتج.",
            "label.saveBenzineHelp": "سيتم استخدامه لحساب صافي الموزّع لليوم.",
            "label.quickDailyEntry": "إدخال مبيعات يومية",
            "label.quickDailyEntrySub": "أضف سطور المبيعات لعميل اليوم",
            "label.quickSummary": "ملخص يومي",
            "label.quickSummarySub": "راجع الإجماليات والأرباح والتوزيعات",
            "label.quickMonthlyDashboard": "لوحة الشهر",
            "label.quickMonthlyDashboardSub": "رسوم وأفضل المنتجات والنتائج الصافية",
            "label.quickProducts": "المنتجات",
            "label.quickProductsSub": "إدارة قواعد التسعير والأرباح",
            "label.quickClients": "العملاء",
            "label.quickClientsSub": "إدارة العملاء والأسعار الافتراضية",
            "label.quickMonthlyCosts": "التكاليف الشهرية",
            "label.quickMonthlyCostsSub": "تتبع المصاريف مثل البنزين وغيرها"
            ,
            "label.editProduct": "تعديل المنتج",
            "label.editClient": "تعديل العميل"
        }
    };

    const translate = (key) => {
        const lang = localStorage.getItem('dfmLang') || 'en';
        const set = translations[lang] || translations.en;
        return set[key] || translations.en[key] || key;
    };

    const applyTranslations = () => {
        const year = new Date().getFullYear();
        document.querySelectorAll('[data-i18n]').forEach((el) => {
            const key = el.getAttribute('data-i18n');
            let value = translate(key);
            if (value && value.includes('{year}')) {
                value = value.replace('{year}', String(year));
            }
            if (value) {
                el.textContent = value;
            }
        });

        document.querySelectorAll('[data-i18n-placeholder]').forEach((el) => {
            const key = el.getAttribute('data-i18n-placeholder');
            const value = translate(key);
            if (value) {
                el.setAttribute('placeholder', value);
            }
        });

        document.querySelectorAll('[data-i18n-aria-label]').forEach((el) => {
            const key = el.getAttribute('data-i18n-aria-label');
            const value = translate(key);
            if (value) {
                el.setAttribute('aria-label', value);
            }
        });

        document.querySelectorAll('[data-i18n-title]').forEach((el) => {
            const key = el.getAttribute('data-i18n-title');
            const value = translate(key);
            if (value) {
                el.setAttribute('title', value);
            }
        });

        document.querySelectorAll('[data-i18n-value]').forEach((el) => {
            const key = el.getAttribute('data-i18n-value');
            const value = translate(key);
            if (value) {
                el.setAttribute('value', value);
            }
        });
    };

    window.dfmTranslate = translate;
    window.dfmApplyTranslations = applyTranslations;

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', applyTranslations);
    } else {
        applyTranslations();
    }
})();

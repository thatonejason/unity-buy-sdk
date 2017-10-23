package com.shopify.unity.buy.androidpay.view.widget;

import android.content.Context;
import android.support.test.InstrumentationRegistry;
import android.support.test.runner.AndroidJUnit4;
import android.view.ContextThemeWrapper;
import android.view.LayoutInflater;

import com.shopify.unity.buy.R;
import com.shopify.unity.buy.androidpay.view.viewmodel.ShippingRatesViewModel;
import com.shopify.unity.buy.models.ShippingMethod;
import com.shopify.unity.buy.utils.ForceLocaleRule;

import org.junit.Before;
import org.junit.ClassRule;
import org.junit.Test;
import org.junit.runner.RunWith;

import java.math.BigDecimal;
import java.util.Collections;
import java.util.List;
import java.util.Locale;

import static org.junit.Assert.assertEquals;

/**
 * @author Flavio Faria
 */
@RunWith(AndroidJUnit4.class)
public class ShippingRatesViewTest {

    @ClassRule
    public static ForceLocaleRule localeTestRule = new ForceLocaleRule(Locale.US);
    private Context themedContext;

    @Before
    public void setUp() throws Exception {
        Context context = InstrumentationRegistry.getContext();
        themedContext = new ContextThemeWrapper(context, R.style.BuyTheme);
    }

    @Test
    public void update() throws Exception {
        ShippingRatesView view = (ShippingRatesView) LayoutInflater
                .from(themedContext)
                .inflate(R.layout.view_confirmation_shipping_rates, null);

        ShippingMethod shippingMethod = new ShippingMethod(
                "identifier",
                "detail",
                "label",
                BigDecimal.valueOf(3.45)
        );
        List<ShippingMethod> shippingMethods = Collections.singletonList(shippingMethod);
        ShippingRatesViewModel model = new ShippingRatesViewModel(shippingMethods, 0);

        view.update(model);
        assertEquals(view.getShippingLineView().getText(), "label");
        assertEquals(view.getPriceView().getText(), "$3.45");
    }
}

import Vue from 'vue'
import Vuelidate from 'vuelidate'
import Paginate from 'vuejs-paginate'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import dateFilter from './filters/date.filter'
import messagePlugin from '@/utils/message.plugin'
import titlePlugin from '@/utils/title.plugin'
import tooltipDirective from '@/directives/tooltip.directive'
import Loader from '@/components/app/Loader'
import store from './store'
import 'materialize-css/dist/js/materialize.min'
import currencyFilter from './filters/currency.filter'
import localizeFilter from './filters/localize.filter'
import VueMeta from 'vue-meta'



Vue.config.productionTip = false

Vue.filter('currency', currencyFilter)
Vue.filter('localize', localizeFilter)
Vue.filter('date', dateFilter)
Vue.use(Vuelidate)
Vue.use(messagePlugin)
Vue.use(titlePlugin)
Vue.use(require('vue-moment'));
Vue.use(VueMeta, {refreshOnceOnNavigation: true})
Vue.component('Loader', Loader)
Vue.component('Paginate', Paginate)
Vue.directive('tooltip', tooltipDirective)

let app = new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')





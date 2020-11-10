<template>
  <div>
    <div class="page-title">
      <h3>{{ "History_Title" | localize }}</h3>
    </div>

    <div class="input-field">
      <input type="date" v-model="startDate" v-on:change="fetchRecords" />
      <label for="limit">{{ "StartDate" | localize }}</label>
    </div>

    <div class="input-field">
      <input type="date" v-model="endDate" v-on:change="fetchRecords" />
      <label for="limit">{{ "EndDate" | localize }}</label>
    </div>

    <div class="history-chart">
      <canvas ref="canvas"></canvas>
    </div>

    <Loader v-if="loading" />

    <p class="center" v-else-if="!records.length">
      {{ "NoRecords" | localize }}.
      <router-link to="/record">{{ "AddFirst" | localize }}</router-link>
    </p>

    <section v-else>
      <HistoryTable :records="items" />

      <Paginate
        v-model="page"
        :page-count="pageCount"
        :click-handler="pageChangeHandler"
        :prev-text="'Back' | localize"
        :next-text="'Forward' | localize"
        :container-class="'pagination'"
        :page-class="'waves-effect'"
      />
    </section>
  </div>
</template>

<script>
import paginationMixin from "@/mixins/pagination.mixin";
import HistoryTable from "@/components/HistoryTable";
import { Pie } from "vue-chartjs";
import localizeFilter from "@/filters/localize.filter";
import moment from "moment";

export default {
  name: "history",
  metaInfo() {
    return {
      title: this.$title("Menu_History"),
    };
  },
  extends: Pie,
  mixins: [paginationMixin],
  data: () => ({
    startDate: moment()
      .startOf("month")
      .format("YYYY-MM-DD"),
    endDate: moment()
      .endOf("month")
      .format("YYYY-MM-DD"),
    loading: true,
    records: [],
  }),
  async mounted() {
    await this.fetchRecords();
  },
  methods: {
    async fetchRecords() {
      this.loading = true;

      this.records = await this.$store.dispatch("fetchRecordsByPeriod", {
        startDate: this.startDate,
        endDate: this.endDate,
      });
      const categoires = await this.$store.dispatch("fetchCategories");

      this.setup(categoires);

      this.loading = false;
    },
    setup(categoires) {
      this.setupPagination(
        this.records.map((record) => {
          return {
            ...record,
            categoryName: categoires.find((c) => c.id === record.categoryId)
              .title,
            typeClass: record.isIncome ? "green" : "red",
            typeText: record.isIncome
              ? localizeFilter("Income")
              : localizeFilter("Outcome"),
          };
        })
      );
      this.renderChart({
        labels: categoires.map((c) => c.title),
        datasets: [
          {
            label: localizeFilter("CostsForCategories"),
            data: categoires.map((c) => {
              return this.records.reduce((total, r) => {
                if (r.categoryId === c.id && r.isIncome == false) {
                  total += +r.amount;
                }
                return total;
              }, 0);
            }),
            backgroundColor: [
              "rgba(255, 99, 132, 0.2)",
              "rgba(54, 162, 235, 0.2)",
              "rgba(255, 206, 86, 0.2)",
              "rgba(75, 192, 192, 0.2)",
              "rgba(153, 102, 255, 0.2)",
              "rgba(255, 159, 64, 0.2)",
            ],
            borderColor: [
              "rgba(255, 99, 132, 1)",
              "rgba(54, 162, 235, 1)",
              "rgba(255, 206, 86, 1)",
              "rgba(75, 192, 192, 1)",
              "rgba(153, 102, 255, 1)",
              "rgba(255, 159, 64, 1)",
            ],
            borderWidth: 1,
          },
        ],
      });
    },
  },
  components: {
    HistoryTable,
  },
};
</script>

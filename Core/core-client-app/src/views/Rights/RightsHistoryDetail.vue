<template>
    <div class="container" align="center">
        <loading :active.sync="isLoading"
                 :can-cancel="false"
                 color="#5c2e91"
                 loader="dots"
                 :is-full-page="true">

        </loading>
        <div class="row">
            <div class="col-12" align="center">
                <h2 id="header2">รายละเอียดการรักษา</h2>
                <div align="left" class="tab-user mb-4 mt-4 px-3" v-for="invdt in invdtData" :key="invdt.invdtId">
                    <p class="inv-header">โรงพยาบาล : {{invdt.hospital}}</p>
                    <p class="inv-header">วันที่เข้ารักษา : {{invdt.takenDate}} เวลา {{invdt.takenTime}} น.</p>
                    <p class="inv-header">จำนวนเงิน : {{invdt.sumMoney}} บาท</p>
                    <table id="treatments" class="mb-4 mt-2">
                        <thead>
                            <tr>
                                <th>รายการ</th>
                                <th>จำนวนเงิน</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="item in invdt.items" :key="item.listNo">
                                <td>{{item.treatName}}</td>
                                <td>{{item.money}} บาท</td>

                            </tr>

                        </tbody>
                    </table>
                </div>              
                <p class="p_right mt-4">รวมจำนวนเงิน: {{sumMoneyTotal}} บาท</p>
            </div>
        </div>
        <br>

    </div>
</template>

<style>
    .inv-header {
        margin-bottom: 0px;

    }
    #treatments {
        border-collapse: collapse;
        width: 100%;
        border-radius: 20px;
    }

        #treatments td, #treatments th {
            border: 1px solid #ccc;
            padding: 8px;
        }

        #treatments tr:nth-child(even) {
            background-color: #ddd;
        }

        #treatments tr:hover {
            background-color: white;
        }

        #treatments th {
            border: 1.8px solid #ddd;
            padding-top: 12px;
            padding-bottom: 12px;
            text-align: center;
            background-color: var(--main-color);
            color: white;
        }
</style>

<script>
    import mixin from '../../mixin/index.js'
    import axios from 'axios'

    // Import loading-overlay
    import Loading from 'vue-loading-overlay';
    import 'vue-loading-overlay/dist/vue-loading.css';

    export default {
        name: 'RightsHistorydetail',
        mixins: [mixin],
        components: {
            Loading
        },
        data() {
            return {
                msg: "ดูเพิ่มเติม",
                msg2: "เบิกค่ารักษาเบื้องต้น",
                userData: this.$store.state.userStateData,
                accData: this.$store.getters.accGetter(this.$route.params.id),
                sumMoneyTotal: this.$route.params.sumMoney,
                invdtData: null,
                isLoading:true
            }
        },
        methods: {
            getInvoicedtDetail() {
                var url = this.$store.state.envUrl + '/api/Approval/Invoicedt/{accNo}/{victimNo}/{appNo}'.replace('{accNo}', this.$route.params.id).replace('{victimNo}', this.$route.params.victimNo).replace('{appNo}', this.$route.params.appNo);
                var apiConfig = {
                    headers: {
                        Authorization: "Bearer " + this.$store.state.jwtToken.token
                    }
                }
                axios.get(url, apiConfig )
                    .then((response) => {
                        this.invdtData = response.data;                       
                        this.isLoading = false
                    })
                    .catch((error) => {
                        if (error.toString().includes("401")) {
                            this.getJwtToken()
                            this.getInvoicedtDetail()
                        }
                    });
            },
        },
        created() {
            this.getInvoicedtDetail();
        }
    }
</script>
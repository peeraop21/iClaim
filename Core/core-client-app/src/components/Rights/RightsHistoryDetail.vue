<template>
    <div class="container" align="center">
        <div class="row">
            <div class="col-12" align="center">
                <h2 id="header2">รายละเอียดการรักษา</h2>
                <div align="left" class="tab-user mb-4 mt-4 px-3">
                    <p>โรงพยาบาลกรุงเทพ</p>
                    <p style="margin-top: -10px; margin-bottom: 0px">วันที่เข้ารักษา : {{ claimData.stringApRegdate }}</p>
                </div>
                <div v-if="formType=='pt3'">
                    <table id="treatments" class="mb-4">
                        <tr>
                            <th>รายการ</th>
                            <th>จำนวนเงิน</th>
                        </tr>
                        <tr>
                            <td>ค่ารักษาพยาบาล</td>
                            <td v-if="accData.lastClaim.cureMoney">{{ accData.lastClaim.cureMoney }} บาท</td>
                            <td v-else-if="!accData.lastClaim.cureMoney">0 บาท</td>
                        </tr>
                        <tr>
                            <td>ค่าสูญเสียอวัยวะ / ทุพพลภาพ</td>
                            <td v-if="accData.lastClaim.crippledMoney">{{ accData.lastClaim.crippledMoney }} บาท</td>
                            <td v-else-if="!accData.lastClaim.crippledMoney">0 บาท</td>
                        </tr>
                        <tr>
                            <td>ค่าอนามัย</td>
                            <td v-if="accData.lastClaim.hygieneMoney">{{ accData.lastClaim.hygieneMoney }} บาท</td>
                            <td v-else-if="!accData.lastClaim.hygieneMoney">0 บาท</td>
                        </tr>
                        <tr>
                            <td>ค่าปลงศพและค่าใช้จ่ายเกี่ยวกับการจัดการศพ</td>
                            <td v-if="accData.lastClaim.deadMoney">{{ accData.lastClaim.deadMoney }} บาท</td>
                            <td v-else-if="!accData.lastClaim.deadMoney">0 บาท</td>
                        </tr>
                    </table>
                </div>
                <div v-if="formType=='pt4'">
                    <table id="treatments" class="mb-4">
                        <tr>
                            <th>รายการ</th>
                            <th>จำนวนเงิน</th>
                        </tr>
                        <tr>
                            <td>ค่ายาและสารบำบัด</td>
                            <td v-if="accData.lastClaim.medicineMoney">{{ accData.lastClaim.medicineMoney }} บาท</td>
                            <td v-else-if="!accData.lastClaim.medicineMoney">0 บาท</td>
                        </tr>
                        <tr>
                            <td>ค่าอวัยวะเทียม</td>
                            <td v-if="accData.lastClaim.plasticMoney">{{ accData.lastClaim.plasticMoney }} บาท</td>
                            <td v-else-if="!accData.lastClaim.plasticMoney">0 บาท</td>
                        </tr>
                        <tr>
                            <td>ค่าบริการทางการแพทย์</td>
                            <td v-if="accData.lastClaim.serviceMoney">{{ accData.lastClaim.serviceMoney }} บาท</td>
                            <td v-else-if="!accData.lastClaim.serviceMoney">0 บาท</td>
                        </tr>
                        <tr>
                            <td>ค่าห้องและค่าอาหาร</td>
                            <td v-if="accData.lastClaim.roomMoney">{{ accData.lastClaim.roomMoney }} บาท</td>
                            <td v-else-if="!accData.lastClaim.roomMoney">0 บาท</td>
                        </tr>
                        <tr>
                            <td>ค่าพาหนะและค่านำส่งสถานพยาบาล</td>
                            <td v-if="accData.lastClaim.veihcleMoney">{{ accData.lastClaim.veihcleMoney }} บาท</td>
                            <td v-else-if="!accData.lastClaim.veihcleMoney">0 บาท</td>
                        </tr>
                    </table>
                </div>
                <p class="p_right">รวมจำนวนเงิน: {{ accData.lastClaim.sumMoney }} บาท</p>

            </div>
        </div>
        <br>
     
    </div>
</template>

<style>
#treatments {
  border-collapse: collapse;
  width: 100%;
  border-radius: 20px;
}

    #treatments td, #treatments th {
        border: 1px solid #ccc;
        padding: 8px;
    }

#treatments tr:nth-child(even){background-color: #ddd;}

#treatments tr:hover {background-color: white;}

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
    export default {
        name: 'RightsHistorydetail',
        /*props: {
            approval: Array
        },*/
        data () {
            return {
                pt3_list: [
                    {
                        id: 1,
                        list: "ค่ารักษาพยาบาล",
                        money: 0
                    },
                    {
                        id: 2,
                        list: "ค่าสูญเสียอวัยวะ",
                        money: 0
                    },
                    {
                        id: 3,
                        list: "ค่าทุพพลภาพ",
                        money: 0
                    },
                    {
                        id: 4,
                        list: "ค่าปลงศพและค่าใช้จ่ายเกี่ยวกับการจัดการศพ",
                        money: 0
                    },

                ],
                treatments: [
                    {
                        id: 1,
                        treatment_list: "ค่ายาและสารบำบัด",
                        money: 100
                  },
                  {
                      id: 2,
                      treatment_list: "ค่าอวัยวะเทียม",
                      money: 0
                  },
                  {
                      id: 3,
                      treatment_list: "ค่าบริการทางการแพทย์",
                      money: 1000
                  },
                  {
                      id: 4,
                      treatment_list: "ค่าห้องและค่าอาหาร",
                      money: 500
                  },
                  {
                      id: 5,
                      treatment_list: "ค่าพาหนะและค่านำส่งสถานพยาบาล",
                      money: 500
                  },
                ],
                msg: "ดูเพิ่มเติม",
                msg2: "เบิกค่ารักษาเบื้องต้น",
                userData: this.$store.state.userStateData,
                accData: this.$store.getters.accGetter(this.$route.params.id),
                claimData: this.$store.getters.ptGetter(this.$route.params.pt),
                formType: this.$route.params.typept,
            }
        },
        
        mounted() {
            console.log("ClaimData", this.claimData);
            console.log("AccData", this.accData);
        }
}
</script>
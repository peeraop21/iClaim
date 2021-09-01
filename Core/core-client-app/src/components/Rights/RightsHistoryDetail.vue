<template>
    <div class="container" align="center">
        <div class="row">
            <div class="col-12" align="center">
                <h2 id="header2">รายละเอียดการรักษา</h2>
                <div align="left" class="tab-user mb-4 mt-4 px-3" v-for="approvals in approval" :key="approvals.crClaimno">
                    <p>โรงพยาบาลกรุงเทพ</p>
                    <p style="margin-top: -10px; margin-bottom: 0px">วันที่เข้ารักษา : {{ approvals.stringApRegdate }}</p>
                </div>
                <table id="treatments" class="mb-4">
                    <tr>
                        <th>รายการ</th>
                        <th>จำนวนเงิน</th>
                    </tr>
                    <tr  v-for="treatment in treatments" :key="treatment.id">
                        <td>{{ treatment.treatment_list }}</td>
                        <td>{{ treatment.money }} บาท</td>
                    </tr>
                </table>
                <p class="p_right">รวมจำนวนเงิน: 10000 บาท</p>

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
                treatments: [
                  {
                      id: 1,
                      treatment_list: "ค่ายาและสารบำบัด",
                      money: 17000
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
                  accData: this.$store.getters.accGetter(this.$route.params.id)
            }
        },
        props: ['approval'],
}
</script>
<template>
    <div class="container" align="center">
        <div class="row">
            <div class="col-12" align="center">
                <h2 id="header2">รายละเอียดการรักษา</h2>
                <div align="left" class="tab-user mb-4 mt-4 px-3">
                    <p>โรงพยาบาล : </p>
                    <p style="margin-top: -10px; margin-bottom: 0px">วันที่เข้ารักษา : {{ claimData.stringApRegdate }}</p>
                </div>
                <div v-if="formTypePT==='pt3' && formTypeRights === 2">
                    <table id="treatments" class="mb-4">
                        <tr>
                            <th>รายการ</th>
                            <th></th>
                        </tr>
                        <tr>
                            <td>ตาบอด</td>
                            <td v-if="accData.lastClaim.blindCrippled === 'Y'"><ion-icon name="checkmark" style="font-size: 20px"></ion-icon></td>
                            <td v-else></td>
                        </tr>
                        <tr>
                            <td>หูหนวก</td>
                            <td v-if="accData.lastClaim.unHearCrippled === 'Y'"><ion-icon name="checkmark" style="font-size: 20px; color: var(--main-color)"></ion-icon></td>
                            <td v-else></td>
                        </tr>
                        <tr>
                            <td>เป็นใบ้ ลิ้นขาด หรือสุญเสียความสามารถในการพูด</td>
                            <td v-if="accData.lastClaim.deafCrippled === 'Y'"><ion-icon name="checkmark" style="font-size: 20px; color: var(--main-color)"></ion-icon></td>
                            <td v-else></td>
                        </tr>
                        <tr>
                            <td>สูญเสียอวัยวะสืบพันธุ์</td>
                            <td v-if="accData.lastClaim.lostSexualCrippled === 'Y'"><ion-icon name="checkmark" style="font-size: 20px; color: var(--main-color)"></ion-icon></td>
                            <td v-else></td>
                        </tr>
                        <tr>
                            <td>สูญเสียแขน ขา มือ เท้า นิ้ว หรืออวัยวะอื่นใด</td>
                            <td v-if="accData.lastClaim.lostOrganCrippled === 'Y'"><ion-icon name="checkmark" style="font-size: 20px; color: var(--main-color)"></ion-icon></td>
                            <td v-else></td>
                        </tr>
                        <tr>
                            <td>จิตพิการอย่างติดตัว</td>
                            <td v-if="accData.lastClaim.lostMindCrippled === 'Y'"><ion-icon name="checkmark" style="font-size: 20px; color: var(--main-color)"></ion-icon></td>
                            <td v-else></td>
                        </tr>
                        <tr>
                            <td>ทุพพลภาพอย่างถาวร</td>
                            <td v-if="accData.lastClaim.crippledPermanent === 'Y'"><ion-icon name="checkmark" style="font-size: 20px; color: var(--main-color)"></ion-icon></td>
                            <td v-else></td>
                        </tr>
                    </table>
                    <label v-if="accData.lastClaim.crippledComment" style="float: left">รายละเอียดเพิ่มเติม: {{ accData.lastClaim.crippledComment }}</label>
                    <label v-else-if="!accData.lastClaim.crippledComment" style="float: left">รายละเอียดเพิ่มเติม: -</label>
                    <br />
                </div>
                <div v-else-if="formTypePT==='pt3' && formTypeRights === 1 && accData.lastClaim.medicineMoney">
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
                <div v-else-if="formTypePT==='pt3' && formTypeRights === 1 && !accData.lastClaim.medicineMoney">
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
                <div v-else-if="formTypePT==='pt4' && !accData.lastClaim.crippledMoney">
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
                <div v-else-if="formTypePT==='pt4' && !accData.lastClaim.medicineMoney">
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
                <p class="p_right mt-4">รวมจำนวนเงิน: {{ accData.lastClaim.sumMoney }} บาท</p>
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
                msg: "ดูเพิ่มเติม",
                msg2: "เบิกค่ารักษาเบื้องต้น",
                userData: this.$store.state.userStateData,
                accData: this.$store.getters.accGetter(this.$route.params.id),
                claimData: this.$store.getters.ptGetter(this.$route.params.pt),
                formTypePT: this.$route.params.typept,
                formTypeRights: this.$route.params.typerights,
            }
        },
        
        mounted() {
            console.log("ClaimData", this.claimData);
            console.log("AccData", this.accData);
        }
}
</script>
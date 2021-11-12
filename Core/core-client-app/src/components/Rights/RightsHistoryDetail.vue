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

                <!--<div v-if="(formTypePT==='pt3' || formTypePT==='KCL') && formTypeRights === 2">
                <table id="treatments" class="mb-4">
                    <tr>
                        <th>รายการ</th>
                        <th></th>
                    </tr>
                    <tr>
                        <td>ตาบอด</td>
                        <td v-if="claimData.claim.blindCrippled === 'Y'">
                            <ion-icon name="checkmark" style="font-size: 20px"></ion-icon>
                        </td>
                        <td v-else></td>
                    </tr>
                    <tr>
                        <td>หูหนวก</td>
                        <td v-if="claimData.claim.unHearCrippled === 'Y'">
                            <ion-icon name="checkmark" style="font-size: 20px; color: var(--main-color)"></ion-icon>
                        </td>
                        <td v-else></td>
                    </tr>
                    <tr>
                        <td>เป็นใบ้ ลิ้นขาด หรือสุญเสียความสามารถในการพูด</td>
                        <td v-if="claimData.claim.deafCrippled === 'Y'">
                            <ion-icon name="checkmark" style="font-size: 20px; color: var(--main-color)"></ion-icon>
                        </td>
                        <td v-else></td>
                    </tr>
                    <tr>
                        <td>สูญเสียอวัยวะสืบพันธุ์</td>
                        <td v-if="claimData.claim.lostSexualCrippled === 'Y'">
                            <ion-icon name="checkmark" style="font-size: 20px; color: var(--main-color)"></ion-icon>
                        </td>
                        <td v-else></td>
                    </tr>
                    <tr>
                        <td>สูญเสียแขน ขา มือ เท้า นิ้ว หรืออวัยวะอื่นใด</td>
                        <td v-if="claimData.claim.lostOrganCrippled === 'Y'">
                            <ion-icon name="checkmark" style="font-size: 20px; color: var(--main-color)"></ion-icon>
                        </td>
                        <td v-else></td>
                    </tr>
                    <tr>
                        <td>จิตพิการอย่างติดตัว</td>
                        <td v-if="claimData.claim.lostMindCrippled === 'Y'">
                            <ion-icon name="checkmark" style="font-size: 20px; color: var(--main-color)"></ion-icon>
                        </td>
                        <td v-else></td>
                    </tr>
                    <tr>
                        <td>ทุพพลภาพอย่างถาวร</td>
                        <td v-if="claimData.claim.crippledPermanent === 'Y'">
                            <ion-icon name="checkmark" style="font-size: 20px; color: var(--main-color)"></ion-icon>
                        </td>
                        <td v-else></td>
                    </tr>
                </table>
                <label v-if="claimData.claim.crippledComment" style="float: left">รายละเอียดเพิ่มเติม: {{ claimData.claim.crippledComment }}</label>
                <label v-else-if="!claimData.claim.crippledComment" style="float: left">รายละเอียดเพิ่มเติม: -</label>
                <br />
            </div>
            <div v-else-if="formTypePT==='pt3' && formTypeRights === 1 && claimData.claim.medicineMoney">
                <table id="treatments" class="mb-4">
                    <tr>
                        <th>รายการ</th>
                        <th>จำนวนเงิน</th>
                    </tr>
                    <tr>
                        <td>ค่ายาและสารบำบัด</td>
                        <td v-if="claimData.claim.medicineMoney">{{ claimData.claim.medicineMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.medicineMoney">0 บาท</td>
                    </tr>
                    <tr>
                        <td>ค่าอวัยวะเทียม</td>
                        <td v-if="claimData.claim.plasticMoney">{{ claimData.claim.plasticMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.plasticMoney">0 บาท</td>
                    </tr>
                    <tr>
                        <td>ค่าบริการทางการแพทย์</td>
                        <td v-if="claimData.claim.serviceMoney">{{ claimData.claim.serviceMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.serviceMoney">0 บาท</td>
                    </tr>
                    <tr>
                        <td>ค่าห้องและค่าอาหาร</td>
                        <td v-if="claimData.claim.roomMoney">{{ claimData.claim.roomMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.roomMoney">0 บาท</td>
                    </tr>
                    <tr>
                        <td>ค่าพาหนะและค่านำส่งสถานพยาบาล</td>
                        <td v-if="claimData.claim.veihcleMoney">{{ claimData.claim.veihcleMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.veihcleMoney">0 บาท</td>
                    </tr>
                </table>
            </div>
            <div v-else-if="formTypePT==='pt3' && formTypeRights === 1 && !claimData.claim.medicineMoney">
                <table id="treatments" class="mb-4">
                    <thead>
                        <tr>
                            <th>รายการ</th>
                            <th>จำนวนเงิน</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>ค่ารักษาพยาบาล</td>
                            <td v-if="claimData.claim.cureMoney">{{ claimData.claim.cureMoney }} บาท</td>
                            <td v-else-if="!claimData.claim.cureMoney">0 บาท</td>
                        </tr>
                        <tr>
                            <td>ค่าสูญเสียอวัยวะ / ทุพพลภาพ</td>
                            <td v-if="claimData.claim.crippledMoney">{{ claimData.claim.crippledMoney }} บาท</td>
                            <td v-else-if="!claimData.claim.crippledMoney">0 บาท</td>
                        </tr>
                        <tr>
                            <td>ค่าอนามัย</td>
                            <td v-if="claimData.claim.hygieneMoney">{{ claimData.claim.hygieneMoney }} บาท</td>
                            <td v-else-if="!claimData.claim.hygieneMoney">0 บาท</td>
                        </tr>
                        <tr>
                            <td>ค่าปลงศพและค่าใช้จ่ายเกี่ยวกับการจัดการศพ</td>
                            <td v-if="claimData.claim.deadMoney">{{ claimData.claim.deadMoney }} บาท</td>
                            <td v-else-if="!claimData.claim.deadMoney">0 บาท</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div v-else-if="formTypePT==='pt4' && !claimData.claim.crippledMoney">
                <table id="treatments" class="mb-4">
                    <tr>
                        <th>รายการ</th>
                        <th>จำนวนเงิน</th>
                    </tr>
                    <tr>
                        <td>ค่ายาและสารบำบัด</td>
                        <td v-if="claimData.claim.medicineMoney">{{ claimData.claim.medicineMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.medicineMoney">0 บาท</td>
                    </tr>
                    <tr>
                        <td>ค่าอวัยวะเทียม</td>
                        <td v-if="claimData.claim.plasticMoney">{{ claimData.claim.plasticMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.plasticMoney">0 บาท</td>
                    </tr>
                    <tr>
                        <td>ค่าบริการทางการแพทย์</td>
                        <td v-if="claimData.claim.serviceMoney">{{ claimData.claim.serviceMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.serviceMoney">0 บาท</td>
                    </tr>
                    <tr>
                        <td>ค่าห้องและค่าอาหาร</td>
                        <td v-if="claimData.claim.roomMoney">{{ claimData.claim.roomMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.roomMoney">0 บาท</td>
                    </tr>
                    <tr>
                        <td>ค่าพาหนะและค่านำส่งสถานพยาบาล</td>
                        <td v-if="claimData.claim.veihcleMoney">{{ claimData.claim.veihcleMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.veihcleMoney">0 บาท</td>
                    </tr>
                </table>
            </div>
            <div v-else-if="formTypePT==='pt4' && !claimData.claim.medicineMoney">
                <table id="treatments" class="mb-4">
                    <tr>
                        <th>รายการ</th>
                        <th>จำนวนเงิน</th>
                    </tr>
                    <tr>
                        <td>ค่ารักษาพยาบาล</td>
                        <td v-if="claimData.claim.cureMoney">{{ claimData.claim.cureMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.cureMoney">0 บาท</td>
                    </tr>
                    <tr>
                        <td>ค่าสูญเสียอวัยวะ / ทุพพลภาพ</td>
                        <td v-if="claimData.claim.crippledMoney">{{ claimData.claim.crippledMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.crippledMoney">0 บาท</td>
                    </tr>
                    <tr>
                        <td>ค่าอนามัย</td>
                        <td v-if="claimData.claim.hygieneMoney">{{ claimData.claim.hygieneMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.hygieneMoney">0 บาท</td>
                    </tr>
                    <tr>
                        <td>ค่าปลงศพและค่าใช้จ่ายเกี่ยวกับการจัดการศพ</td>
                        <td v-if="claimData.claim.deadMoney">{{ claimData.claim.deadMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.deadMoney">0 บาท</td>
                    </tr>
                </table>
            </div>
            <div v-else-if="formTypePT==='KCL' && !claimData.claim.medicineMoney">
                <table id="treatments" class="mb-4">
                    <tr>
                        <th>รายการ</th>
                        <th>จำนวนเงิน</th>
                    </tr>
                    <tr>
                        <td>ค่ารักษาพยาบาล</td>
                        <td v-if="claimData.claim.cureMoney">{{ claimData.claim.cureMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.cureMoney">0 บาท</td>
                    </tr>
                    <tr>
                        <td>ค่าสูญเสียอวัยวะ / ทุพพลภาพ</td>
                        <td v-if="claimData.claim.crippledMoney">{{ claimData.claim.crippledMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.crippledMoney">0 บาท</td>
                    </tr>
                    <tr>
                        <td>ค่าอนามัย</td>
                        <td v-if="claimData.claim.hygieneMoney">{{ claimData.claim.hygieneMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.hygieneMoney">0 บาท</td>
                    </tr>
                    <tr>
                        <td>ค่าปลงศพและค่าใช้จ่ายเกี่ยวกับการจัดการศพ</td>
                        <td v-if="claimData.claim.deadMoney">{{ claimData.claim.deadMoney }} บาท</td>
                        <td v-else-if="!claimData.claim.deadMoney">0 บาท</td>
                    </tr>
                </table>
            </div>-->
                
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
    import axios from 'axios'

    // Import loading-overlay
    import Loading from 'vue-loading-overlay';
    import 'vue-loading-overlay/dist/vue-loading.css';

    export default {
        name: 'RightsHistorydetail',
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
                /*claimData: this.$store.getters.ptGetter(this.$route.params.pt),*/
                //formTypePT: this.$route.params.typept,
                //formTypeRights: this.$route.params.typerights,
                invdtData: null,
                isLoading:true
            }
        },
        methods: {
            getInvoicedtDetail() {
                console.log('getInvoicedtDetail');
                var url = this.$store.state.envUrl + '/api/Approval/Invoicedt/{accNo}/{victimNo}/{appNo}'.replace('{accNo}', this.$route.params.id).replace('{victimNo}', this.$route.params.victimNo).replace('{appNo}', this.$route.params.appNo);
                axios.get(url)
                    .then((response) => {
                        this.invdtData = response.data;
                        
                        console.log("invDetail: ", this.invdtData);
                        this.isLoading = false
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
        },
        created() {
            this.getInvoicedtDetail();
        }
    }
</script>
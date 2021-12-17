<template>
    <div class=" container" align="center">
        <loading :active.sync="isLoading"
                 :can-cancel="false"
                 color="#5c2e91"
                 loader="dots"
                 :is-full-page="true">

        </loading>
        <div class="row">
            <div class="col-12" align="center">
                <h2 id="header2">ประวัติการใช้สิทธิ์</h2>
                <br>
                <div class="tab-user">
                    <div class="row">
                        <div class="col-3 mt-1 px-0">
                            <img src="@/assets/notify.png" width="75">
                        </div>
                        <div class="col-9 text-start px-1">
                            <!--<span>ชื่อ-สกุล: {{userData.prefix}}{{userData.fname}} {{userData.lname}}</span><br />
                            <span>เลขประจำตัวประชาชน: {{userData.idcardNo}}</span><br />-->
                            <span>เลขที่รับแจ้ง: <span style="color: var(--main-color)">{{accData.accNo}}</span></span>
                            <br />
                            <span>ทะเบียนรถที่เกิดเหตุ : <span v-for="(car, index) in accData.car" :key="`car-${index}`" style="color: var(--main-color)">{{car}} </span></span>
                            <br />
                            <span>วันที่เกิดเหตุ : <span style="color: var(--main-color)">{{ accData.stringAccDate }}</span></span>
                            <br />
                            <div v-if="formType==1">
                                <span>สิทธิ์ที่ได้รับ: <span style="color: var(--main-color)">30000 บาท</span></span>
                                <br />
                                <span>สิทธิ์ที่ใช้ไป: <span style="color: var(--main-color)">{{cureRightsUsed}} บาท</span></span>
                                <br />
                                <span v-if="accData.cureRightsBalance >= 0">สิทธิ์คงเหลือ: <span style="color: var(--main-color)">{{ accData.cureRightsBalance }} บาท</span></span>
                                <span v-if="accData.cureRightsBalance < 0">สิทธิ์คงเหลือ: <span style="color: var(--main-color)">0 บาท</span></span>
                            </div>
                            <div v-if="formType==2">
                                <span>สิทธิ์ที่ได้รับ: <span style="color: var(--main-color)">35000 บาท</span></span>
                                <br />
                                <span>สิทธิ์ที่ใช้ไป: <span style="color: var(--main-color)">{{crippledRightsUsed}} บาท</span></span>
                                <br />
                                <span v-if="accData.crippledRightsBalance >= 0">สิทธิ์คงเหลือ: <span style="color: var(--main-color)">{{ accData.crippledRightsBalance }} บาท</span></span>
                                <span v-if="accData.crippledRightsBalance < 0">สิทธิ์คงเหลือ: <span style="color: var(--main-color)">0 บาท</span></span>
                            </div>

                        </div>
                    </div>
                </div>
                <br>
                <br>
                <div v-if="approval.length == 0">
                    <p class="notData">- ไม่มีข้อมูลประวัติ -</p>
                </div>
                <div v-else-if="formType==1">
                    <div align="left" style="width: 100%;">
                        <label>ประวัติการใช้สิทธิ์กรณีเบิกค่ารักษาพยาบาล</label>
                        <br>
                    </div>
                    <div v-for="approvals in approval" :key="approvals.pt4">
                        <!--<div v-if="!approvals.claim.accNo">
                        </div>
                        <div v-else-if="approvals.claim.crippledMoney > 0 && approvals.claim.cureMoney == 0">
                        </div>-->
                        <div>
                            <section>
                                <div style="height: 100%; width: 100%;">
                                    <div class="accordion">
                                        <div class="accordion-item" :id="'list' + approvals.pt4">
                                            <a class="accordion-link" :href="'#list' + approvals.pt4">
                                                <div>
                                                    <p>
                                                        <ion-icon name="document-text-outline"></ion-icon>{{ approvals.pt4 }}
                                                        <br>
                                                        <ion-icon name="card-outline"></ion-icon>จำนวนเงิน: {{ approvals.apTotal }} บาท
                                                    </p>
                                                </div>
                                                <ion-icon name="chevron-down-outline" class="icon ion-md-add"></ion-icon>
                                            </a>
                                            <div class="answer">
                                                <p v-if="approvals.apStatus==='A'">สถานะการจ่ายเงิน: อนุมัติ</p>
                                                <p v-else-if="approvals.apStatus==='P'">สถานะการจ่ายเงิน: จ่ายแล้ว</p>
                                                <p v-else>สถานะการจ่ายเงิน: -</p>
                                                <p style="margin-top: -25px;">
                                                    วันที่ใช้สิทธิ์: {{ approvals.stringApRegdate }}
                                                </p>
                                            </div>
                                            <div style="text-align: center">
                                                <router-link class="btn-select" :to="{ name: 'RightsHistoryDetail', params: { id: accData.stringAccNo, appNo: approvals.accAppNo, victimNo: approvals.accVictimNo, typerights: 1,sumMoney: approvals.apTotal}}">ดูเพิ่มเติม</router-link>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </div>
                    </div>
                </div>
                <div v-else-if="formType==2">
                    <div align="left" style="width: 100%;">
                        <label>ประวัติการใช้สิทธิ์กรณีเบิกค่าสูญเสียอวัยวะ/ทุพพลภาพ</label>
                        <br>
                    </div>
                    <!--<div v-for="approvals in approval" :key="approvals.pt4">
                        <div v-if="approvals.subPt4 === 'pt4' || !approvals.claim.crippledMoney || approvals.claim.crippledMoney === 0 || !approvals.claim.accNo">
                        </div>
                        <div v-else-if="approvals.claim.crippledMoney > 0 ">
                            <section>
                                <div style="height: 100%; width: 100%;">
                                    <div class="accordion">
                                        <div class="accordion-item" :id="'list' + approvals.pt4">
                                            <a class="accordion-link" :href="'#list' + approvals.pt4">
                                                <div>
                                                    <p>
                                                        <ion-icon name="document-text-outline"></ion-icon>{{ approvals.pt4 }}
                                                        <br>
                                                        <ion-icon name="card-outline"></ion-icon>จำนวนเงิน: {{ approvals.apTotal }} บาท
                                                    </p>
                                                </div>
                                                <ion-icon name="chevron-down-outline" class="icon ion-md-add"></ion-icon>
                                            </a>
                                            <div class="answer">
                                                <p v-if="approvals.apStatus==='A'">สถานะการจ่ายเงิน: อนุมัติ</p>
                                                <p v-else-if="approvals.apStatus==='P'">สถานะการจ่ายเงิน: จ่ายแล้ว</p>
                                                <p v-else>สถานะการจ่ายเงิน: -</p>
                                                <p style="margin-top: -25px;">
                                                    วันที่ใช้สิทธิ์: {{ approvals.stringApRegdate }}
                                                </p>
                                                <p style="margin-top: -25px;">โรงพยาบาลที่รักษา:</p>
                                            </div>
                                            <div style="text-align: center">
                                                <router-link class="btn-select" :to="{ name: 'RightsHistoryDetail', params: { id: accData.stringAccNo, typerights: 2, pt: approvals.stringPt4, typept: approvals.subPt4 }}">ดูเพิ่มเติม</router-link>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </div>
                        <div v-else>
                        </div>
                    </div>-->
                </div>
            </div>
        </div>
        <br>
    </div>
</template>

<script>
    import mixin from '../../mixin/index.js'
    import axios from 'axios'
    //import RightsHistoryDetail from "@/components/Rights/RightsHistoryDetail.vue";

    // Import loading-overlay
    import Loading from 'vue-loading-overlay';
    import 'vue-loading-overlay/dist/vue-loading.css';

    export default {
        name: 'RightsHistory',
        mixins: [mixin],
        components: {
            Loading
        },
        data() {
            return {
                userData: this.$store.state.userStateData,
                approval: [],
                accData: this.$store.getters.accGetter(this.$route.params.id),
                formType: this.$route.params.typerights,
                cureRightsUsed: 30000,
                crippledRightsUsed: 35000,
                isLoading: true

            }
        },
        methods: {
            getApprovals() {
                console.log('getApproval');
                var url = this.$store.state.envUrl + '/api/Approval/{accNo}/{victimNo}/{rightsType}'.replace('{accNo}', this.accData.stringAccNo).replace('{victimNo}', this.accData.victimNo).replace('{rightsType}', this.$route.params.typerights);
                var apiConfig = {
                    headers: {
                        Authorization: "Bearer " + this.$store.state.jwtToken.token
                    }
                }
                axios.get(url, apiConfig)
                    .then((response) => {
                        this.$store.state.claimStateData = response.data;
                        this.approval = this.$store.state.claimStateData;
                        console.log("claimData", this.approval);
                        this.isLoading = false
                    })
                    .catch((error) => {
                        if (error.toString().includes("401")) {
                            this.getJwtToken()
                            this.getApprovals()
                        }
                    });
            },
            calRightsUsed() {
                if (this.accData.cureRightsBalance >= 0) {
                    this.cureRightsUsed = this.cureRightsUsed - parseFloat(this.accData.cureRightsBalance)
                }
                if (this.accData.crippledRightsBalance >= 0) {
                    this.crippledRightsUsed = this.crippledRightsUsed - parseFloat(this.accData.crippledRightsBalance)
                }

            }
        },
        mounted() {
            this.calRightsUsed();
            this.getApprovals();
            console.log("AccData", this.accData);
        }

    }
</script>

<style>
    p.notData {
        margin-top: 20px;
        color: #bbb;
        font-size: 20px;
        font-weight: bold;
    }
</style>

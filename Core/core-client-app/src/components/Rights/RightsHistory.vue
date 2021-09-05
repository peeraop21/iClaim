<template>
    <div class=" container" align="center" >
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
                            <span>เลขที่รับแจ้ง: <span style="color: var(--main-color)">{{accData.accNo}}</span></span><br />
                            <span>ทะเบียนรถที่เกิดเหตุ : <span v-for="(car, index) in accData.car" :key="`car-${index}`" style="color: var(--main-color)">{{car}} </span></span><br />
                            <span>วันที่เกิดเหตุ : <span style="color: var(--main-color)">{{ accData.stringAccDate }}</span></span><br />
                            <span>สิทธิ์ที่ได้รับ: <span style="color: var(--main-color)">35000 บาท</span></span><br />
                            <span>สิทธิ์ที่ใช้ไป: <span style="color: var(--main-color)">25000 บาท</span></span><br />
                            <span>สิทธิ์คงเหลือ: <span style="color: var(--main-color)">10000 บาท</span></span>
                        </div>
                    </div>
                </div>
                <br>
                <br>
                <div v-if="!accData.lastClaim.claimNo">
                    <p class="notData">- ไม่มีข้อมูลประวัติ -</p>
                </div>
                <div v-else-if="formType==1">
                    <div align="left" style="width: 100%;">
                        <label>ประวัติการใช้สิทธิ์กรณีเบิกค่ารักษาพยาบาล</label>
                        <br>
                    </div>
                    <section>
                        <div style="height: 100%; width: 100%;">
                            <div class="accordion" v-for="approvals in approval" :key="approvals.stringPt4">
                                <div class="accordion-item" :id="'list' + approvals.stringPt4">
                                    <a class="accordion-link" :href="'#list' + approvals.stringPt4">
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
                                        <router-link class="btn-select" :to="{ name: 'RightsHistoryDetail', params: { id: accData.stringAccNo, typerights: 1, pt: approvals.stringPt4, typept: approvals.subPt4 }}">ดูเพิ่มเติม</router-link>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                   
                </div>
                <div v-else-if="formType==2">
                    <div align="left" style="width: 100%;">
                        <label>ประวัติการใช้สิทธิ์กรณีเบิกค่าสูญเสียอวัยวะ/ทุพพลภาพ</label>
                        <br>
                    </div>
                    <div v-if="approval.subPt4 === 'pt4' || !accData.lastClaim.crippledMoney || accData.lastClaim.crippledMoney === 0">
                        <p class="notData">- ไม่มีข้อมูล -</p>
                    </div>
                    <div v-else-if="accData.lastClaim.crippledMoney > 0 ">
                        <section>
                            <div style="height: 100%; width: 100%;">
                                <div class="accordion" v-for="approvals in approval" :key="approvals.stringPt4">
                                    <div class="accordion-item" :id="'list' + approvals.stringPt4">
                                        <a class="accordion-link" :href="'#list' + approvals.stringPt4">
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
                        <p class="notData">- ไม่มีข้อมูล -</p>
                    </div>
                </div>
            </div>
        </div>
        <br>
        
    </div>
</template>

<script>
    import axios from 'axios'
    //import RightsHistoryDetail from "@/components/Rights/RightsHistoryDetail.vue";
   
    export default {
        name: 'RightsHistory',
        /*components: {
            RightsHistoryDetail
        },*/
        data() {
            return {
                userData: this.$store.state.userStateData,
                approval: [],
                accData: this.$store.getters.accGetter(this.$route.params.id),
                formType: this.$route.params.typerights,
            }
        },
        methods: {
            getApprovals() {
                console.log('getApproval');
                var url = '/api/Approval/{accNo}'.replace('{accNo}', this.accData.stringAccNo);
                axios.get(url)
                    .then((response) => {
                        this.$store.state.claimStateData = response.data;
                        this.approval = this.$store.state.claimStateData;
                        console.log("claimData", this.approval);

                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
        },
        mounted() {
            console.log(this.accData);
            this.getApprovals();
            
        }

    }
</script>

<style>
    p.notData{
        margin-top: 20px;
        color: #bbb;
        font-size: 20px;
        font-weight: bold;
    }
</style>

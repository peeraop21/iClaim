<template>
    <div>

        <vs-dialog width="550px" not-center v-model="active">
            <template #header>
                <h4 class="not-margin">
                    คำรับรอง
                </h4>
            </template>
            <div class="con-content" align="left">
                <div class="d-block text-left">
                    <p>
                        ข้าพเจ้าผู้ยื่นคำร้องขอในนามของผู้ประสบภัย ขอให้ค้ารับรองว่า.-
                        <br>
                        1. ข้าพเจ้าหรือประสบภัยยังไม่เคยรับหรือทำสัญญาว่าจะรับค่าเสียหายจากเจ้าของรถ หรือผู้หนึ่งผู้ใด
                        ใด และยังไม่เคยรับหรือยื่นขอรับค่าเสียหายเบื้องต้นจากกองทุนทดแทนผู้ประสบภัย
                    </p>
                    <p>
                        2. ข้าพเจ้าหรือผู้ประสบภัย
                    </p>
                    <div style="margin-top: -10px;">
                        <vs-radio color="var(--main-color)" v-model="picked" val="1" class="mb-1" style="float: left">
                            ไม่เคย&emsp;&emsp;
                        </vs-radio>
                        <vs-radio color="#7d33ff" v-model="picked" val="2" style="float: left">
                            เคย
                        </vs-radio>
                    </div>
                    <br />
                    <div class="mt-0" v-if="picked==='2'">
                        <br />
                        <label>จำนวนเงิน</label>
                        <vs-input color="var(--main-color)"
                                  v-model="value7"
                                  placeholder="จำนวนเงิน" />
                        <label>สถานพยาบาลชื่อ</label>
                        <vs-input color="var(--main-color)"
                                  v-model="value7"
                                  placeholder="สถานพยาบาล" />

                    </div>
                    <br />
                    <p style="margin-top: -15px">
                        มอบอำนาจให้สถานพยาบาลขอรับค่าเสียหายเบื้องต้น
                    </p>
                    <p class="mt-3">
                        3. เมื่อข้าพเจ้าได้รับค่าเสียหายเบื้องต้นจากบริษัทประกันภัยครบถ้วนตามจำนวนที่กฎหมายกำหนดแล้ว
                        ข้าพเจ้าขอสัญญาว่า ข้าพเจ้าจะไม่ไปเรียกร้องค่าเสียหายเบื้องต้นจากเจ้าของรถ หรือมอบอำนาจให้บุคคลอื่น
                        หรือสถานพยาบาลมารับค่าเสียหายเบื้องต้นจำนวนนี้ซ้ำอีก
                    </p>
                    <div class="row">
                        <div class="col-2" style="padding-right: 0px; width:13%;">
                            <vs-checkbox color="#7d33ff" v-model="acceptData"></vs-checkbox>
                        </div>
                        <div class="col-10 px-0">
                            <p class="form-check-label" for="flexCheckDefault" style="text-align:start">
                                ข้าพเจ้าขอรับรองว่าข้อมูลดังกล่าวข้างต้นเป็นจริงทุกประการ หากข้าพเจ้าผิดคำรับรอง
                                ข้าพเจ้ายินยอมรับผิดในความเสียหายที่เกิดขึ้นทั้งหมดแก่บริษัท
                            </p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-2" style="padding-right: 0px; width:13%;">
                            <vs-checkbox color="#7d33ff" v-model="acceptClaim"></vs-checkbox>
                        </div>
                        <div class="col-10 px-0">
                            <p class="form-check-label" for="flexCheckDefault" style="text-align:start">
                                ข้าพเจ้ารับทราบว่าผู้ประสบภัยผู้ใดยื่นคำร้องขอรับค่าเสียหายเบื้องต้น ตามพระราชบัญญัตินี้โดยทุจริต หรือแสดงหลักฐานอันเป็นเท็จ
                                เพื่อขอรับค่าเสียหายเบื้องต้นตามพระราชบัญญัตินี้ ต้องระวางโทษจำคุกไม่เกินห้าปี หรือปรับไม่เกินแสนบาท หรือทั้งจำทั้งปรับ
                            </p>
                        </div>
                    </div>
                    <div v-if="acceptClaim && acceptData" class="mb-4 mt-2" align="center">
                        <router-link class="btn-next" :to="{ name: 'ConfirmOTP', params: { id: accData.eaTmpId}}">ยืนยันส่งคำร้อง</router-link>
                    </div>
                </div>
            </div>
        </vs-dialog>
        <form-wizard title="" subtitle="" color="#5c2e91" step-size="xs" style="margin-top: -35px;" next-button-text="ดำเนินการต่อ" back-button-text="ย้อนกลับ" finish-button-text="ส่งคำร้อง" @on-complete="active=!active">
            <tab-content title="สร้างคำร้อง" icon="ti ti-pencil-alt">
                <!--:before-change="processFilePageOne"-->
                <div class="" v-if="formType == 2">
                    <div align="left">
                        <label>เอกสารประกอบคำร้องกรณีเบิกค่าสูญเสียอวัยวะ/ทุพพลภาพ</label>
                    </div>

                    <div class="box-container">
                        <p class="mb-0">ใบรับรองแพทย์</p>
                        <div>
                            <file-pond name="idCardFile"
                                       ref="pond"
                                       label-idle="กดที่นี่เพื่ออัพโหลดใบรับรองแพทย์"
                                       credits="null"
                                       v-bind:allow-multiple="false"
                                       v-bind:allowFileEncode="true"
                                       accepted-file-types="image/jpeg, image/png"
                                       v-bind:files="idCardFile"
                                       v-on:addfile="onAddIdCardFile" />
                        </div>
                    </div>

                    <br>
                    <p class="p_right" style="font-size: 15px; font-weight: bold;">รวมจำนวนเงิน: {{ total_amount }} บาท</p>
                </div>
                <div class="" v-if="formType == 1">
                    <div align="left">
                        <label>เอกสารประกอบคำร้องกรณีเบิกค่ารักษาพยาบาลเบื้องต้น</label>
                    </div>

                    <!--<div class="box-container">
                    <p class="mb-0">สำเนาบัตรประจำตัวประชาชน</p>
                    <div>-->
                    <!--<input type="file" name="filename">-->
                    <!--<file-pond name="idCardFile"
                    ref="pond"
                    label-idle="กดที่นี่เพื่ออัพโหลดสำเนาบัตรประชาชน"
                    credits="null"
                    v-bind:allow-multiple="false"
                    v-bind:allowFileEncode="true"
                    accepted-file-types="image/jpeg, image/png"
                    v-bind:files="idCardFile"
                    v-on:addfile="onAddIdCardFile" />-->
                    <!--<input type="submit"> -->
                    <!--</div>
                    </div>-->
                    <br>
                    <div class="box-container">
                        <label class="px-2">อาการบาดเจ็บ</label>
                        <b-form-input class="mt-0 mb-2" v-model="injuri" placeholder=""></b-form-input>

                        <div v-for="(input, index) in bills" :key="`Bill-${index}`" class="input wrapper flex items-center">
                            <p class="px-2 mb-0">ใบเสร็จรับเงินค่ารักษาพยาบาล</p>
                            <!--<input type="file" @change="onFileChange">-->
                            <file-pond credits="null"
                                       label-idle="กดที่นี่เพื่ออัพโหลดใบเสร็จค่ารักษา"
                                       v-bind:allow-multiple="false"
                                       v-bind:allowFileEncode="true"
                                       accepted-file-types="image/jpeg, image/png"
                                       v-model="input.file" />


                            <!--<b-form-input class="mt-0 mb-2" v-model="input.hospital" type="text" placeholder="" @click="dialogHospital=!dialogHospital" />-->
                            <!--<label class="px-2">จังหวัดของโรงพยาบาล</label>
                            <div class="mb-2">
                                <select v-model="selectChangwat.changwat">
                                    <option v-for="changwat in changwats" :value="changwat.changwatshortname" v-bind:key="changwat.changwatshortname" style="font-size: 12px; line-height: 0px">
                                        {{ changwat.changwatname }}
                                    </option>
                                </select>
                            </div>
                            <label class="px-2">โรงพยาบาล</label>
                            <div class="mb-2">
                                <select v-model="selectHospital.hospital">
                                    <option v-for="hospital in hospitals" :value="hospital.hospitalid" v-bind:key="hospital.hospitalid" style="font-size: 12px; line-height: 0px">
                                        {{ hospital.hospitaltradename }}
                                    </option>
                                </select>
                            </div>-->
                            <!-- Dialog Hospital -->
                            <!--<label class="px-2">จังหวัดของโรงพยาบาล</label>
                            <div class="mb-2">
                                <select name="category" id="category" v-model="selectChangwat">
                                    <option v-for="(category, index) in changwats" :value="category.changwatshortname" :key="index" style="font-size: 12px; line-height: 0px">
                                        {{ category.changwatname }}
                                    </option>
                                </select>
                            </div>-->

                            <vs-dialog width="550px" not-center v-model="modalHospital">
                                <template #header>
                                    <h4 class="not-margin">
                                        เลือกโรงพยาบาล
                                    </h4>
                                </template>
                                <div class="con-content" align="left">
                                    <div class="d-block text-left">
                                        <div class="mb-2">
                                            <label class="px-2">จังหวัด</label>
                                            <select name="category" id="category" v-model="selectChangwat" @change="onChangwatChange">
                                                <option v-for="(category, index) in changwats" :value="category.changwatshortname" :key="index" style="font-size: 12px; line-height: 0px">
                                                    {{ category.changwatname }}
                                                </option>
                                            </select>
                                        </div>
                                        <div class="mb-2" v-show="divHospitalModal">
                                            <label class="px-2">โรงพยาบาล</label>
                                            <select name="item" id="item" v-model="mockHospital">
                                                <option v-for="(item, index) in filteredItems" :value="item.hospitaltradename" :key="index">
                                                    {{ item.hospitaltradename }}
                                                </option>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                                <template #footer>
                                    <div class="footer-dialog">
                                        <vs-button block @click="submitModalHospital(index)" >
                                            ยืนยัน
                                        </vs-button>

                                    </div>
                                </template>
                            </vs-dialog>
                            <label class="px-2">โรงพยาบาล</label>
                            <b-form-input class="mt-0 mb-2" v-model="input.selectHospital" type="text" @click="modalHospital=!modalHospital" />
                            <label class="px-2">เลขที่ใบเสร็จ</label>
                            <b-form-input class="mt-0 mb-2" v-model="input.bill_no" type="text" placeholder="" />
                            <label class="px-2">จำนวนเงิน</label>
                            <b-form-input class="mt-0 mb-2" v-model="input.money" type="number" placeholder="" @change="calMoney" />
                            <label class="px-2">วันที่เข้ารักษา</label>
                            <b-form-datepicker v-model="input.hospitalized_date"
                                               selected-variant="primary"
                                               label-selected=""
                                               label-no-date-selected=""
                                               :close-button="true"
                                               :label-help="null"
                                               label-close-button="ปิด"
                                               :today-button="true"
                                               label-today-button="เลือกวันปัจจุบัน"
                                               :date-format-options="{ year: 'numeric', month: 'numeric', day: 'numeric' }"
                                               size="sm"
                                               class="mt-0 mb-2 " locale="th" placeholder=""></b-form-datepicker>
                            <br>
                            <!-- Add Svg Icon-->
                            <p style="color: green">
                                <svg @click="addField(input, bills)"
                                     xmlns="http://www.w3.org/2000/svg"
                                     viewBox="0 0 30 30"
                                     width="30"
                                     height="30"
                                     class="ml-2 cursor-pointer">
                                    <path fill="none" d="M0 0h24v24H0z" />
                                    <path fill="green" d="M11 11V7h2v4h4v2h-4v4h-2v-4H7v-2h4zm1 11C6.477 22 2 17.523 2 12S6.477 2 12 2s10 4.477 10 10-4.477 10-10 10zm0-2a8 8 0 1 0 0-16 8 8 0 0 0 0 16z" />
                                </svg>เพิ่มใบเสร็จ
                            </p>
                            <!-- Remove Svg Icon-->
                            <svg v-show="bills.length > 1"
                                 @click="removeField(index, bills)"
                                 xmlns="http://www.w3.org/2000/svg"
                                 viewBox="0 0 30 30"
                                 width="30"
                                 height="30"
                                 class="ml-2 cursor-pointer mb-2"
                                 style="margin-top: -10px;">
                                <path fill="none" d="M0 0h24v24H0z" />
                                <path fill="#EC4899" d="M12 22C6.477 22 2 17.523 2 12S6.477 2 12 2s10 4.477 10 10-4.477 10-10 10zm0-2a8 8 0 1 0 0-16 8 8 0 0 0 0 16zm0-9.414l2.828-2.829 1.415 1.415L13.414 12l2.829 2.828-1.415 1.415L12 13.414l-2.828 2.829-1.415-1.415L10.586 12 7.757 9.172l1.415-1.415L12 10.586z" />
                            </svg>

                        </div>

                    </div>
                    <br>
                    <br>
                    <p class="p_right" style="font-size: 15px; font-weight: bold;">รวมจำนวนเงิน: {{ total_amount }} บาท</p>
                </div>
            </tab-content>
            <!-- บัญชีรับเงิน -->
            <tab-content title="บัญชีรับเงิน" icon="ti ti-money">
                <div>
                    <p for="my-file" align="left" class="title-advice-menu mb-0">ภาพถ่ายหน้าสมุดบัญชีธนาคาร</p>
                    <div class="box-container mt-0">
                        <div class="form-group ">

                            <file-pond name="bankFile"
                                       ref="pond"
                                       credits="null"
                                       label-idle="กดที่นี่เพื่ออัพโหลดรูปบัญชีธนาคาร"
                                       v-bind:allow-multiple="false"
                                       accepted-file-types="image/jpeg, image/png"
                                       v-bind:files="bankFile"
                                       v-on:addfile="onAddBankAccountFile" />
                            <!--<input type="file" accept="image/*" @change="previewImage" class="form-control-file" id="my-file" style="margin-top: -10px">
                            <div class="border p-2 mt-2">
                                <p align="left">ภาพถ่ายที่เลือก:</p>
                                <template v-if="preview">
                                    <img :src="preview" class="img-fluid" style="width: 30%" />-->
                            <!--<p class="mb-0">ชื่อไฟล์: {{ image.name }}</p>
                            <p class="mb-0">size: {{ image.size/1024 }}KB</p>-->
                            <!--</template>
                            </div>-->
                        </div>

                        <div>

                            <p class="mb-0 px-2">ชื่อธนาคาร</p>
                            <div class="mb-2">
                                <select v-model="selectBank.bankName" style="font-size: 13px;">
                                    <option v-for="bankName in bankNames" :value="bankName.name" v-bind:key="bankName.bank" style="font-size: 12px; line-height: 0px">
                                        {{ bankName.name }}
                                    </option>
                                </select>
                            </div>
                            <p class="mb-0 px-2">ชื่อบัญชีธนาคาร</p>
                            <b-form-input class="mt-0 mb-2" v-model="accountName" placeholder=""></b-form-input>
                            <p class="mb-0 px-2">เลขบัญชีธนาคาร</p>
                            <b-form-input class="mt-0 mb-2" v-model="accountNumber" placeholder=""></b-form-input>
                        </div>
                        <br>
                    </div>
                </div>
                <br>
            </tab-content>
            <!-- ผู้ประสบภัย -->
            <tab-content title="ส่งคำร้อง" icon="ti ti-share">
                <div align="left" style="width: 100%;">
                    <ion-icon name="people-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                    <label align="left" class="title-advice-menu mb-1">ข้อมูลผู้ประสบภัย</label>
                </div>
                <div class="box-container mb-3">
                    <div class="row">
                        <div class="col-9">
                            <p class="mb-0">ชื่อ-สกุล</p>
                            <div class="mt-0" v-if="userData.prefix != null">
                                <p class="mb-0" style="color: grey">{{userData.prefix}}{{userData.fname}} {{userData.lname}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.prefix === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-3">
                            <p class="mb-0">อายุ</p>
                            <div class="mt-0" v-if="bank!=''">
                                <p class="mb-0" style="color: grey">{{bank}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="bank===''">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-4">
                            <p class="mb-0">บ้านเลขที่</p>
                            <div class="mt-0" v-if="userData.homeHouseNo != null">
                                <p class="mb-0" style="color: grey">{{userData.homeHouseNo}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.homeHouseNo === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-2">
                            <p class="mb-0">หมู่</p>
                            <div class="mt-0" v-if="userData.homeHmo != null">
                                <p class="mb-0" style="color: grey">{{userData.homeHmo}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.homeHmo === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-6">
                            <p class="mb-0">ซอย</p>
                            <div class="mt-0" v-if="userData.homeSoi != null">
                                <p class="mb-0" style="color: grey">{{userData.homeSoi}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.homeSoi === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6">
                            <p class="mb-0">ถนน</p>
                            <div class="mt-0" v-if="userData.homeRoad != null">
                                <p class="mb-0" style="color: grey">{{userData.homeRoad}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.homeRoad === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-6">
                            <p class="mb-0">ตำบล/แขวง</p>
                            <div class="mt-0" v-if="userData.homeTumbolId != null">
                                <p class="mb-0" style="color: grey">{{userData.homeTumbolId}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.homeTumbolId === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6">
                            <p class="mb-0">อำเภอ</p>
                            <div class="mt-0" v-if="userData.homeCityId != null">
                                <p class="mb-0" style="color: grey">{{userData.homeCityId}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.homeCityId === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-6">
                            <p class="mb-0">จังหวัด</p>
                            <div class="mt-0" v-if="userData.homeProvinceId != null">
                                <p class="mb-0" style="color: grey">{{userData.homeProvinceId}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.homeProvinceId === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6">
                            <p class="mb-0">รหัสไปรษณีย์</p>
                            <div class="mt-0" v-if="userData.homeZipcode != null">
                                <p class="mb-0" style="color: grey">{{userData.homeZipcode}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.homeZipcode === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-6">
                            <p class="mb-0">เบอร์โทรศัพท์</p>
                            <div class="mt-0" v-if="userData.mobileNo != null">
                                <p class="mb-0" style="color: grey">{{userData.mobileNo}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="userData.mobileNo === null">
                                <p class="mb-0" style="color: grey">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                </div>

                <!-- เหตุ -->
                <div align="left" style="width: 100%;">
                    <ion-icon name="bicycle-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                    <label align="left" class="title-advice-menu mb-1">ข้อมูลอุบัติเหตุ</label>
                </div>
                <div class="box-container mb-3">

                    <p class="mb-0">วันที่เกิดเหตุ</p>
                    <div class="mt-0" v-if="accData.eaAccDate != null">
                        <p class="mb-0" style="color: grey">{{accData.stringAccDate}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="accData.eaAccDate === null">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">ลักษณะเกิดเหตุ</p>
                    <div class="mt-0" v-if="bank!=''">
                        <p class="mb-0" style="color: grey">{{bank}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="bank===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">สถานที่เกิดเหตุ</p>
                    <div class="mt-0" v-if="bank!=''">
                        <p class="mb-0" style="color: grey">{{bank}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="bank===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">หมายเลขทะเบียนรถคันเอาประกันภัย</p>
                    <div class="mt-0" v-if="bank!=''">
                        <p class="mb-0" style="color: grey">{{bank}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="bank===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">เลขตัวถัง</p>
                    <div class="mt-0" v-if="bank!=''">
                        <p class="mb-0" style="color: grey">{{bank}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="bank===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">กรมธรรม์คุ้มครองภัยจากรถ เลขที่</p>
                    <div class="mt-0" v-if="bank!=''">
                        <p class="mb-0" style="color: grey">{{bank}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="bank===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>

                </div>

                <!-- เอกสาร -->
                <div align="left" style="width: 100%;">
                    <ion-icon name="reader-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                    <label align="left" class="title-advice-menu mb-1">ข้อมูลเอกสารประกอบคำร้อง</label>
                </div>
                <div class="box-container mb-3">
                    <p class="mb-0">อาการบาดเจ็บ</p>
                    <div class="mt-0" v-if="injuri!=''">
                        <p class="mb-0" style="color: grey">{{injuri}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="injuri===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">ประเภทผู้ป่วย</p>
                    <div class="mt-0" v-if="patientType!=''">
                        <p class="mb-0" style="color: grey">{{patientType}}</p>
                        <hr>
                    </div>
                    <div class="mt-0" v-else-if="patientType===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>

                    <p class="mb-0">สำเนาบัตรประจำตัวประชาชน</p>
                    <div v-if="idCardFileDisplay" align="center">
                        <img class="img-show" :src="idCardFileDisplay.base64" />
                        <br />
                        <label>{{idCardFileDisplay.filename}}</label>
                    </div>

                    <div class="card-bill" v-for="bill in bills" :key="bill.billNo">
                        <p class="mb-2">ใบเสร็จรับเงินค่ารักษาพยาบาล</p>
                        <div v-if="bill.BillfileShow" align="center">
                            <img class="img-show" :src="bill.BillfileShow" />
                            <br />
                            <label>{{bill.filename}}</label>
                        </div>
                        <!--<div class="mt-0" v-if="bill.BillfileShow!=''">
                            <p class="mb-0" style="color: grey">{{bill.BillfileShow}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="bill.BillfileShow===''">
                            <p class="mb-0" style="color: grey">-</p>
                            <hr class="mt-0">
                        </div>-->
                        <p class="mb-0">ชื่อโรงพยาบาล</p>
                        <div class="mt-0" v-if="bill.selectHospital!=''">
                            <p class="mb-0" style="color: grey">{{ bill.selectHospital }}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="bill.selectHospital===''">
                            <p class="mb-0" style="color: grey">-</p>
                            <hr class="mt-0">
                        </div>
                        <div class="row">
                            <div class="col-6">
                                <p class="mb-0">เลขที่ใบเสร็จ</p>
                                <div class="mt-0" v-if="bill.bill_no!=''">
                                    <p class="mb-0" style="color: grey">{{bill.bill_no}}</p>
                                    <hr class="mt-0">
                                </div>
                                <div class="mt-0" v-else-if="bill.bill_no===''">
                                    <p class="mb-0" style="color: grey">-</p>
                                    <hr class="mt-0">
                                </div>
                            </div>
                            <div class="col-6">
                                <p class="mb-0">วันที่เข้ารักษา</p>
                                <div class="mt-0" v-if="bill.hospitalized_date!=''">
                                    <p class="mb-0" style="color: grey">{{bill.hospitalized_date}}</p>
                                    <hr class="mt-0">
                                </div>
                                <div class="mt-0" v-else-if="bill.hospitalized_date===''">
                                    <p class="mb-0" style="color: grey">-</p>
                                    <hr class="mt-0">
                                </div>
                            </div>
                        </div>
                        <p class="mb-0">จำนวนเงิน</p>
                        <div class="mt-0" v-if="bill.money!=''">
                            <p class="mb-0" style="color: grey">{{bill.money}} บาท</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="bill.money===''">
                            <p class="mb-0" style="color: grey">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>

                    <p class="mb-0">จำนวนเงินรวมทั้งหมด</p>
                    <div class="mt-0" v-if="total_amount!=''">
                        <p class="mb-0" style="color: grey">{{total_amount}} บาท</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="total_amount===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>
                </div>
                <!-- บัญชี -->
                <div align="left" style="width: 100%;">
                    <ion-icon name="card-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                    <label align="left" class="title-advice-menu mb-1">ข้อมูลบัญชีรับเงิน</label>
                </div>
                <div class="box-container mb-3">
                    <p class="mb-0">หน้าสมุดบัญชีธนาคาร</p>

                    <div v-if="bankFileDisplay" align="center">
                        <img class="img-show" :src="bankFileDisplay.base64" />
                        <br />
                        <label>{{bankFileDisplay.filename}}</label>
                    </div>
                    <!--<div class="mt-0" v-if="bankFileShow!=''">
                        <p class="mb-0" style="color: grey">{{image.name}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-if="bankFileShow === ''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>-->
                    <p class="mb-0">ชื่อธนาคาร</p>
                    <div class="mt-0" v-if="selectBank.bankName!=''">
                        <p class="mb-0" style="color: grey">{{ selectBank.bankName }}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="selectBank.bankName===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>

                    <p class="mb-0">ชื่อบัญชีธนาคาร</p>
                    <div class="mt-0" v-if="accountName!=''">
                        <p class="mb-0" style="color: grey">{{accountName}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="accountName===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">เลขบัญชีธนาคาร</p>
                    <div class="mt-0" v-if="accountNumber!=''">
                        <p class="mb-0" style="color: grey">{{accountNumber}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="accountNumber===''">
                        <p class="mb-0" style="color: grey">-</p>
                        <hr class="mt-0">
                    </div>

                    <!-- <div class="form-check">
                    <input class="form-check-input" type="checkbox" v-model="acceptClaim">
                    <p class="form-check-label" for="flexCheckDefault" style="text-align:start">
                        ข้าพเจ้าตรวจสอบและยืนยันข้อมูลทุกอย่างเป็นความจริง
                    </p>
                    </div> -->
                </div>
            </tab-content>

        </form-wizard>
    </div>
</template>

<script>
    import axios from 'axios'
    import { FormWizard, TabContent } from 'vue-form-wizard'
    import 'vue-form-wizard/dist/vue-form-wizard.min.css'
    import vueFilePond from 'vue-filepond';
    

    // Import FilePond styles
    import 'filepond/dist/filepond.min.css'

    // Import FilePond plugins
    // Please note that you need to install these plugins separately

    // Import image preview plugin styles
    import 'filepond-plugin-image-preview/dist/filepond-plugin-image-preview.min.css';

    // Import image preview and file type validation plugins
    import FilePondPluginFileValidateType from 'filepond-plugin-file-validate-type';
    import FilePondPluginImagePreview from 'filepond-plugin-image-preview';
    import FilePondPluginFileEncode from 'filepond-plugin-file-encode';


    // Create component
    const FilePond = vueFilePond(FilePondPluginFileValidateType, FilePondPluginImagePreview, FilePondPluginFileEncode);


    //Your Javascript lives within the Script Tag
    export default {
        name: "Claim",
        components: {
            FormWizard,
            TabContent,
            FilePond
        },
        data() {
            return {
                // ---Bill
                injuri: '',
                patientType: '',
                bills: [{ billNo: 1, bill_no: "", selectHospital: '', money: "", hospitalized_date: "", file: null, BillfileShow: "", filename: "" }],
                total_amount: null,
                // --BookBank
                bank: '',
                bookbank: '',
                accountName: '',
                accountNumber: '',
                phoneNumbers: [{ phone: "" }],
                image: '',
                preview: null,
                // ---Preview
                acceptClaim: false,
                acceptData: false,
                selected: 'first',
                options: [
                    { text: ' ไม่เคย', value: 'first' },
                    { text: ' เคย', value: 'second' },
                ],
                userData: this.$store.state.userStateData,
                accData: this.$store.getters.accGetter(this.$route.params.id),
                formType: this.$route.params.type,
                // bankData: this.$store.state.bankStateData,
                idCardFile: null,
                billsFile: null,
                bankFile: null,
                idCardFileDisplay: { file: null, filename: "", base64: "" },
                bankFileDisplay: { file: null, filename: "", base64: "" },
                // ----Dialog
                active: false,
                dialogHospital: false,
                // Radio in Dialog
                picked: 1,
                //----Get Bank Name
                bankNames: [],
                selectBank: {
                    bankName: ''
                },
                //----Get Hospital Name
                hospitals: [],
                //selectHospital: '',
                //----Get Changwat Name
                changwats: [],
                selectChangwat: '',
                modalHospital: false,
                mockHospital: '',
                divHospitalModal:false
            };
        },

        methods: {
            getBankNames() {
                console.log('getBankNames');
                var url = '/api/Master/Bank';
                axios.get(url)
                    .then((response) => {
                        //this.$store.state.bankStateData = response.data;
                        //this.bankData = this.$store.state.bankStateData
                        //console.log(this.$store.state.bankStateData);
                        this.bankNames = response.data;
                        console.log(response.data);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            /*getHospitalNames() {
                console.log('getHospitalNames');
                var url = '/api/Master/Hospital';
                axios.get(url)
                    .then((response) => {
                        this.hospitals = response.data;
                        console.log(response.data);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },*/
            getHospitalNames() {
                console.log('getHospitalNames');
                var url = "https://ts2thairscapi.rvpeservice.com/3PAccidentAPI/api/Utility/Hospital";
                axios.post(url)
                    .then((response) => {
                        this.hospitals = response.data.data;
                        console.log(this.hospitals);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getChangwatNames() {
                console.log('getChangwatNames');  
                var url = '/api/Master/Changwat';
                axios.get(url)
                    .then((response) => {
                        this.changwats = response.data;
                        console.log(response.data);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            onAddIdCardFile: function (error, file) {
                this.idCardFileDisplay.file = file
                this.idCardFileDisplay.filename = file.filename
                this.idCardFileDisplay.base64 = file.getFileEncodeDataURL()
            },
            onAddBankAccountFile: function (error, file) {
                this.bankFileDisplay.file = file
                this.bankFileDisplay.filename = file.filename
                this.bankFileDisplay.base64 = file.getFileEncodeDataURL()
            },
            processFilePageOne() {
                for (let i = 0; i < this.bills.length; i++) {
                    this.bills[i].BillfileShow = this.bills[i].file[0].getFileEncodeDataURL()
                    this.bills[i].filename = this.bills[i].file[0].filename
                }
                return true;
            },
            //getIt: function () {
            //    console.log(this.$refs.pond.getFiles());
            //},
            onChangwatChange() {

                this.divHospitalModal = true;
                
            },
            submitModalHospital(index) {
                this.bills[index].selectHospital = this.mockHospital
                this.modalHospital = false
                this.selectChangwat = 0;
                this.mockHospital = 0;
                this.divHospitalModal = false;
            },
            calMoney() {
                let sum = 0;
                for (let i = 0; i < this.bills.length; i++) {

                    sum = sum + parseInt(this.bills[i].money)
                }
                this.total_amount = sum

            },
            addField(value, fieldType) {
                var index = this.bills.length + 1
                fieldType.push({ billNo: index, bill_no: "", selectHospital: '', money: "", hospitalized_date: "", file: null, BillfileShow: "", filename: "" });
                console.log(this.bills)
            },
            removeField(index, fieldType) {
                //type.splice(index, 1);
                fieldType.splice(index, 1);
                console.log(this.bills)
            },
            onFileChange(event) {
                var files = event.target.files || event.dataTransfer.files;
                if (!files.length)
                    return;
                console.log(files[0]);
            },
            createImage(file) {
                var Image = new Image();
                var reader = new FileReader();
                var vm = this;

                reader.onload = (e) => {
                    vm.image = e.target.result;
                };
                reader.readAsDataURL(file);
            },
            removeImage: function () {
                this.image = '';
            },
            // --- BookBank
            previewImage: function (event) {
                var input = event.target;
                if (input.files) {
                    var reader = new FileReader();
                    reader.onload = (e) => {
                        this.preview = e.target.result;
                        console.log(this.preview)
                    }
                    this.image = input.files[0];
                    reader.readAsDataURL(input.files[0]);
                }
            },
            //---Alert
            onComplete: function () {
                alert("Thank you for your response! We will contact you soon.");
            },
        },
        mounted() {
            console.log(this.accData)
            console.log(this.userData)
            this.getBankNames();
            this.getHospitalNames();
            this.getChangwatNames();
            this.selectChangwat = 0;
            this.selectHospital = 0;
            
        },
        computed: {
            filteredItems: function () {
                return this.hospitals.filter(function (el) {
                    return el.CHANGWATSHORTNAME === this.selectChangwat;
                }, this);
            }
        }
    };
</script>

<style>
    .vue-form-wizard.xs .wizard-icon-circle {
        background-color: #f0f0f0;
    }

    .vue-form-wizard .wizard-icon-circle {
        border: 2.5px solid #bdbdbd;
        /*border: none;*/
    }

    .vue-form-wizard .wizard-nav-pills > li > a {
        color: #9c9c9c;
    }

    div.wizard-footer-left {
        float: left;
        background-color: transparent;
    }

    .vue-form-wizard .wizard-btn {
        border-radius: 10px;
        font-size: 13px;
    }

    .vue-form-wizard .wizard-icon-circle .wizard-icon-container {
        border-radius: 50%;
        padding: 20px 10px;
        margin-top: -2px;
    }

    .vue-form-wizard .wizard-btn .wizard-footer-left {
        color: red;
    }

    .vue-form-wizard .wizard-nav-pills a, .vue-form-wizard .wizard-nav-pills li {
        flex: 0px;
    }

    .filepond--drop-label label {
        font-family: 'Mitr';
        font-size: 14px;
    }

    .filepond--drop-label {
        border-style: outset;
        border-radius: 7px;
        border-color: #bdbdbd;
    }

    input[type=number] {
        width: 100%;
        padding: 0px 5px;
        margin: 0px 0px;
        box-sizing: border-box;
        border: 2px solid #bbbbbb;
        border-radius: 4px;
        margin-top: -5px;
        margin-bottom: 10px;
    }

    input[type=date] {
        width: 100%;
        padding: 0px 5px;
        margin: 0px 0px;
        box-sizing: border-box;
        border: 2px solid #bbbbbb;
        border-radius: 4px;
        margin-top: -5px;
        margin-bottom: 10px;
    }

    .b-form-btn-label-control.form-control {
        border: 2px solid #bbbbbb;
        border-radius: 4px;
    }

        .b-form-btn-label-control.form-control > .dropdown-menu {
            padding: 0.5rem;
            background-color: #f8f9fa;
        }

    .img-show {
        margin: 5px;
        width: 30%;
    }

    .card-file {
        border: 1px dashed gray;
        opacity: 0.80;
        border-radius: 10px;
        padding: 5px;
        margin-bottom: 10px;
    }

    .card-bill {
        border: 1px solid #cccccc;
        border-radius: 10px;
        padding: 10px;
        margin-bottom: 10px;
    }

    #bv-modal {
        font-family: "Mitr";
    }

    .d-block {
        font-size: 14px;
        line-height: 20px;
    }

    .box-container {
        background-color: white;
        border: none;
    }
    /* Input */
    .mt-0.mb-2.form-control {
        background-color: #e1deec;
        border: none;
        border-radius: 7px;
        padding: 5px 10px;
        outline: none;
        font-size: 13px;
    }

        .mt-0.mb-2.form-control:hover {
            border: none;
            box-shadow: 1px 2px var(--main-color);
            transform: translateX(1px);
            outline: none;
        }

    select {
        width: 100%;
        background-color: #e1deec;
        border: none;
        border-radius: 7px;
        padding: 8px 10px;
        padding-right: 30px;
        outline: none;
        -webkit-appearance: none;
        -moz-appearance: none;
        background-image: url("data:image/svg+xml;utf8,<svg fill='black' height='24' viewBox='0 0 24 24' width='24' xmlns='http://www.w3.org/2000/svg'><path d='M7 10l5 5 5-5z'/><path d='M0 0h24v24H0z' fill='none'/></svg>");
        background-repeat: no-repeat;
        background-position-x: 98%;
        background-position-y: 50%;
    }

        select:hover {
            border: none;
            box-shadow: 1px 2px var(--main-color);
            transform: translateX(1px);
            outline: none;
        }
</style>
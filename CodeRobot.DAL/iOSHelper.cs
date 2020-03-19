using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace CodeRobot.DAL
{
    public class iOSHelper
    {
        /// <summary>
        /// 生成iOS客户端 Model实体类
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strProjectName"></param>
        /// <param name="strTableName"></param>
        /// <param name="strPackage"></param>
        public static void CreateModelsClass(string strFilePath, string strProjectName, string strTableName, string strPackage)
        {
            try
            {
                //读取版权信息
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");
                string strCompany = iniFile.GetString("COPYRIGHT", "COMPANY", "");
                string strAuthor = iniFile.GetString("COPYRIGHT", "AUTHOR", "");
                string strVersion = iniFile.GetString("COPYRIGHT", "VERSION", "");
                string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");
                string strCreateDate = iniFile.GetString("BASE", "CREATE_DATE", "");

                string strClassName = CommonHelper.GetClassName(strTableName);//类名
                strClassName = CommonHelper.GetTableNameUpper(strClassName);//如：News,NewsType
                string strTableNameLower = CommonHelper.GetTableNameFirtWordLower(strTableName);//如：news,newstype
                string strGetPrimaryKey = CommonHelper.GetPrimaryKey(strTableName);

                //***.h
                Directory.CreateDirectory(strFilePath);
                StreamWriter sw = new StreamWriter(strFilePath + "\\" + strClassName + ".h", false, Encoding.GetEncoding("utf-8"));
                sw.WriteLine("//");
                sw.WriteLine("//  " + strClassName + ".h");
                sw.WriteLine("//  " + strProjectName);
                sw.WriteLine("//");
                sw.WriteLine("//  Created by " + strAuthor + " on " + DateTime.Now.ToString("yyyy/M/d") + ".");
                sw.WriteLine("//  Copyright © " + DateTime.Now.Year + "年 " + strCompany + ". All rights reserved.");
                sw.WriteLine("//");
                sw.WriteLine("\r\n");
                sw.WriteLine("#import <Foundation/Foundation.h>");
                sw.WriteLine("#import \"HtmlMedia.h\"");
                sw.WriteLine("\r\n");
                sw.WriteLine("@interface " + strClassName + " : NSObject");
                sw.WriteLine("\r\n");
                sw.WriteLine(GetDefineParaListForiOS(strTableName) + "\r\n");
                sw.WriteLine("\r\n");
                sw.WriteLine("@property (assign, nonatomic) BOOL canLoadMore, willLoadMore, isLoading;");
                sw.WriteLine("@property (readwrite, nonatomic, strong) HtmlMedia *htmlMedia;");
                sw.WriteLine("\r\n");
                sw.WriteLine("@property (assign, nonatomic) CGFloat contentHeight;");
                sw.WriteLine("\r\n");
                sw.WriteLine("@end");
                sw.Close();

                //***.m
                Directory.CreateDirectory(strFilePath);
                StreamWriter sw2 = new StreamWriter(strFilePath + "\\" + strClassName + ".m", false, Encoding.GetEncoding("utf-8"));
                sw2.WriteLine("//");
                sw2.WriteLine("//  " + strClassName + ".m");
                sw2.WriteLine("//  " + strProjectName);
                sw2.WriteLine("//");
                sw2.WriteLine("//  Created by " + strAuthor + " on " + DateTime.Now.ToString("yyyy/M/d") + ".");
                sw2.WriteLine("//  Copyright © " + DateTime.Now.Year + "年 " + strCompany + ". All rights reserved.");
                sw2.WriteLine("//");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("#import \"" + strClassName + ".h\"");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("@implementation " + strClassName);
                sw2.WriteLine("\r\n");
                sw2.WriteLine("-(id)copyWithZone:(NSZone*)zone {");
                sw2.WriteLine("    " + strClassName + " *" + strTableNameLower + " = [[[self class] allocWithZone:zone] init];");
                sw2.WriteLine(GetSaveModelListForiOS(strTableName));
                sw2.WriteLine("    return " + strTableNameLower + ";");
                sw2.WriteLine("}");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("- (instancetype)init");
                sw2.WriteLine("{\r\n");
                sw2.WriteLine("    self = [super init];");
                sw2.WriteLine("    if (self) {");
                sw2.WriteLine("        _canLoadMore = YES;");
                sw2.WriteLine("        _isLoading = _willLoadMore = NO;");
                sw2.WriteLine("        _contentHeight = 1;");
                sw2.WriteLine("    }");
                sw2.WriteLine("    return self;");
                sw2.WriteLine("}");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("- (void)setContent:(NSString *)content{");
                sw2.WriteLine("    if (_content != content) {");
                sw2.WriteLine("        _htmlMedia = [HtmlMedia htmlMediaWithString:content showType:MediaShowTypeNone];");
                sw2.WriteLine("        _content = _htmlMedia.contentDisplay;");
                sw2.WriteLine("    }");
                sw2.WriteLine("}");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("@end");
                sw2.Close();


                //***Model.h
                Directory.CreateDirectory(strFilePath);
                StreamWriter sw3 = new StreamWriter(strFilePath + "\\" + strClassName + "Model.h", false, Encoding.GetEncoding("utf-8"));
                sw3.WriteLine("//");
                sw3.WriteLine("//  " + strClassName + "Model.h");
                sw3.WriteLine("//  " + strProjectName);
                sw3.WriteLine("//");
                sw3.WriteLine("//  Created by " + strAuthor + " on " + DateTime.Now.ToString("yyyy/M/d") + ".");
                sw3.WriteLine("//  Copyright © " + DateTime.Now.Year + "年 " + strCompany + ". All rights reserved.");
                sw3.WriteLine("//");
                sw3.WriteLine("\r\n");
                sw3.WriteLine("#import <Foundation/Foundation.h>");
                sw3.WriteLine("#import \"HtmlMedia.h\"");
                sw3.WriteLine("#import \"" + strClassName + ".h\"");
                sw3.WriteLine("\r\n");
                sw3.WriteLine("@class " + strClassName + ";");
                sw3.WriteLine("\r\n");
                sw3.WriteLine("typedef NS_ENUM(NSInteger, " + strClassName + "Type)");
                sw3.WriteLine("{");
                sw3.WriteLine("    " + strClassName + "TypeAll = 0,");
                sw3.WriteLine("    " + strClassName + "Type1,");
                sw3.WriteLine("    " + strClassName + "Type2,");
                sw3.WriteLine("    " + strClassName + "Type3,");
                sw3.WriteLine("};");
                sw3.WriteLine("\r\n");
                sw3.WriteLine("@interface " + strClassName + "Model : NSObject");
                sw3.WriteLine("\r\n");
                sw3.WriteLine("@property (strong, nonatomic, readonly) NSDate *last_time;");
                sw3.WriteLine("@property (readonly, nonatomic, strong) NSNumber *last_id;");
                sw3.WriteLine("@property (assign, nonatomic) BOOL canLoadMore, willLoadMore, isLoading;");
                sw3.WriteLine("@property (assign, nonatomic) " + strClassName + "Type type;");
                sw3.WriteLine("@property (readwrite, nonatomic, strong) NSMutableArray *list;");
                sw3.WriteLine("@property (readwrite, nonatomic, strong) NSDictionary *propertyArrayMap;");
                sw3.WriteLine("@property (readwrite, nonatomic, strong) NSNumber *totalPage, *totalRow;");
                sw3.WriteLine("@property (readwrite, nonatomic, strong) NSNumber *page, *pageSize;");
                sw3.WriteLine("\r\n");
                sw3.WriteLine("+ (" + strClassName + "Model *)dataWithType:(" + strClassName + "Type)type;");
                sw3.WriteLine("- (void)configWithData:(NSArray *)responseA;");
                sw3.WriteLine("- (NSString *)toPath;");
                sw3.WriteLine("- (NSNumber *)toPage;");
                sw3.WriteLine("- (NSDictionary *)toParams;");
                sw3.WriteLine("\r\n");
                sw3.WriteLine("@end");
                sw3.Close();

                //***Model.m
                Directory.CreateDirectory(strFilePath);
                StreamWriter sw4 = new StreamWriter(strFilePath + "\\" + strClassName + "Model.m", false, Encoding.GetEncoding("utf-8"));
                sw4.WriteLine("//");
                sw4.WriteLine("//  " + strClassName + "Model.m");
                sw4.WriteLine("//  " + strProjectName);
                sw4.WriteLine("//");
                sw4.WriteLine("//  Created by " + strAuthor + " on " + DateTime.Now.ToString("yyyy/M/d") + ".");
                sw4.WriteLine("//  Copyright © " + DateTime.Now.Year + "年 " + strCompany + ". All rights reserved.");
                sw4.WriteLine("//");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("#import \"" + strClassName + "Model.h\"");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("@implementation " + strClassName + "Model");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("- (instancetype)init");
                sw4.WriteLine("{\r\n");
                sw4.WriteLine("    self = [super init];");
                sw4.WriteLine("    if (self) {");
                sw4.WriteLine("        _page = @1;");
                sw4.WriteLine("        _pageSize = @20;");
                sw4.WriteLine("        _type = " + strClassName + "TypeAll;");
                sw4.WriteLine("        _propertyArrayMap = [NSDictionary dictionaryWithObjectsAndKeys:@\"" + strClassName + "\", @\"list\", nil];");
                sw4.WriteLine("    }");
                sw4.WriteLine("    return self;");
                sw4.WriteLine("}");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("+ (" + strClassName + "Model *)dataWithType:(" + strClassName + "Type)type{");
                sw4.WriteLine("    " + strClassName + "Model *data = [[" + strClassName + "Model alloc] init];");
                sw4.WriteLine("    data.type = type;");
                sw4.WriteLine("    data.canLoadMore = NO;");
                sw4.WriteLine("    data.isLoading = NO;");
                sw4.WriteLine("    data.willLoadMore = NO;");
                sw4.WriteLine("    return data;");
                sw4.WriteLine("}");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("- (NSString *)toPath{");
                sw4.WriteLine("    NSString *requstPath = @\"api/" + strTableNameLower + "\";");
                sw4.WriteLine("    return requstPath;");
                sw4.WriteLine("}");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("- (NSNumber*)toPage{ ");
                sw4.WriteLine("    return _willLoadMore? [NSNumber numberWithInteger:_page.integerValue +1]: [NSNumber numberWithInteger:1];");
                sw4.WriteLine("}");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("- (NSDictionary *)toParams{");
                sw4.WriteLine("    NSMutableDictionary *params = [[NSMutableDictionary alloc] initWithCapacity:2];");
                sw4.WriteLine("    switch (_type) {");
                sw4.WriteLine("        case " + strClassName + "Type1:");
                sw4.WriteLine("            [params setObject:@\"hot\" forKey:@\"sort\"];");
                sw4.WriteLine("            break;");
                sw4.WriteLine("        case " + strClassName + "Type2:");
                sw4.WriteLine("            [params setObject:@\"time\" forKey:@\"sort\"];");
                sw4.WriteLine("            break;");
                sw4.WriteLine("        case " + strClassName + "Type3:");
                sw4.WriteLine("            break;");
                sw4.WriteLine("        default:");
                sw4.WriteLine("            break;");
                sw4.WriteLine("    }");
                sw4.WriteLine("   params[@\"last_time\"] = _willLoadMore? @((NSUInteger)([_last_time timeIntervalSince1970] * 1000)): nil;    params[@\"last_id\"] = _willLoadMore? _last_id: nil;");
                sw4.WriteLine("    return params;");
                sw4.WriteLine("}");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("- (void)configWithData:(NSArray *)responseA{");
                sw4.WriteLine("    if (responseA && [responseA count] > 0) {");
                sw4.WriteLine("        self.canLoadMore = (_type != " + strClassName + "Type1);");
                sw4.WriteLine("        " + strClassName + " *lastData = [responseA lastObject];");
                sw4.WriteLine("        _last_time = lastData.created_at;");
                sw4.WriteLine("        _last_id = lastData." + strGetPrimaryKey + ";");
                sw4.WriteLine("        if (_willLoadMore) {");
                sw4.WriteLine("            [_list addObjectsFromArray:responseA];");
                sw4.WriteLine("        }else{");
                sw4.WriteLine("            self.list = [NSMutableArray arrayWithArray:responseA];");
                sw4.WriteLine("        }");
                sw4.WriteLine("    }else{");
                sw4.WriteLine("        self.canLoadMore = NO;");
                sw4.WriteLine("        if (!_willLoadMore) {");
                sw4.WriteLine("            self.list = [NSMutableArray array];");
                sw4.WriteLine("        }");
                sw4.WriteLine("    }");
                sw4.WriteLine("}");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("@end");
                sw4.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ModelHelper), ex, "生成iOS客户端 Models", "CreateiOSModelsClass",false);
            }
        }

        /// <summary>
        /// 生成iOS客户端 Views
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strProjectName"></param>
        /// <param name="strTableName"></param>
        /// <param name="strPackage"></param>
        public static void CreateViewsClass(string strFilePath, string strProjectName, string strTableName, string strPackage)
        {
            try
            {
                //读取版权信息
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");
                string strCompany = iniFile.GetString("COPYRIGHT", "COMPANY", "");
                string strAuthor = iniFile.GetString("COPYRIGHT", "AUTHOR", "");
                string strVersion = iniFile.GetString("COPYRIGHT", "VERSION", "");
                string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");
                string strCreateDate = iniFile.GetString("BASE", "CREATE_DATE", "");

                string strClassName = CommonHelper.GetClassName(strTableName);//类名
                strClassName = CommonHelper.GetTableNameUpper(strClassName);//如：News,NewsType
                string strTableNameLower = CommonHelper.GetTableNameFirtWordLower(strTableName);//如：news,newstype

                //***ListCell.h
                Directory.CreateDirectory(strFilePath);
                StreamWriter sw = new StreamWriter(strFilePath + "\\" + strClassName + "ListCell.h", false, Encoding.GetEncoding("utf-8"));
                sw.WriteLine("//");
                sw.WriteLine("//  " + strClassName + "ListCell.h");
                sw.WriteLine("//  " + strProjectName);
                sw.WriteLine("//");
                sw.WriteLine("//  Created by " + strAuthor + " on " + DateTime.Now.ToString("yyyy/M/d") + ".");
                sw.WriteLine("//  Copyright © " + DateTime.Now.Year + "年 " + strCompany + ". All rights reserved.");
                sw.WriteLine("//");
                sw.WriteLine("\r\n");
                sw.WriteLine("#define kCellIdentifier_List @\"" + strClassName + "ListCell\"");
                sw.WriteLine("\r\n");
                sw.WriteLine("#import <UIKit/UIKit.h>");
                sw.WriteLine("#import \"" + strClassName + ".h\"");
                sw.WriteLine("#import \"UITapImageView.h\"");
                sw.WriteLine("\r\n");
                sw.WriteLine("@interface " + strClassName + "ListCell : UITableViewCell");
                sw.WriteLine("\r\n");
                sw.WriteLine("@property (nonatomic, copy) void(^goToDetailBlock) (" + strClassName + " *curData);");
                sw.WriteLine("@property (copy, nonatomic) void (^cellRefreshBlock)();");
                sw.WriteLine("@property (nonatomic, assign) NSInteger outIndex;");
                sw.WriteLine("\r\n");
                sw.WriteLine("- (void)setData:(" + strClassName + " *)data;");
                sw.WriteLine("\r\n");
                sw.WriteLine("@end");
                sw.Close();

                //***ListCell.m
                Directory.CreateDirectory(strFilePath);
                StreamWriter sw2 = new StreamWriter(strFilePath + "\\" + strClassName + "ListCell.m", false, Encoding.GetEncoding("utf-8"));
                sw2.WriteLine("//");
                sw2.WriteLine("//  " + strClassName + "ListCell.m");
                sw2.WriteLine("//  " + strProjectName);
                sw2.WriteLine("//");
                sw2.WriteLine("//  Created by " + strAuthor + " on " + DateTime.Now.ToString("yyyy/M/d") + ".");
                sw2.WriteLine("//  Copyright © " + DateTime.Now.Year + "年 " + strCompany + ". All rights reserved.");
                sw2.WriteLine("//");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("#import \"" + strClassName + "ListCell.h\"");
                sw2.WriteLine("#import \"Coding_NetAPIManager.h\"");
                sw2.WriteLine("#import \"MJPhotoBrowser.h\"");
                sw2.WriteLine("#import \"CodingShareView.h\"");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("@interface " + strClassName + "ListCell ()");
                sw2.WriteLine("@property (strong, nonatomic) " + strClassName + " *data;");
                sw2.WriteLine("@property (strong, nonatomic) UITapImageView *coverImgView;");
                sw2.WriteLine("@property (strong, nonatomic) UILabel *titleLabel,*authorLabel;");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("@end");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("@implementation " + strClassName + "ListCell");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("- (id)initWithStyle:(UITableViewCellStyle)style reuseIdentifier:(NSString *)reuseIdentifier");
                sw2.WriteLine("{");
                sw2.WriteLine("    self = [super initWithStyle:style reuseIdentifier:reuseIdentifier];");
                sw2.WriteLine("    if (self) {");
                sw2.WriteLine("        // Initialization code");
                sw2.WriteLine("        self.selectionStyle = UITableViewCellSelectionStyleNone;");
                sw2.WriteLine("        self.backgroundColor = [UIColor clearColor];");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("        // 样式1 图片左侧+文字右侧");
                sw2.WriteLine("        //图片");
                sw2.WriteLine("        if (!self.coverImgView) {");
                sw2.WriteLine("            self.coverImgView = [[UITapImageView alloc] initWithFrame:CGRectMake(10, 10, 80, 60)];");
                sw2.WriteLine("             //[self.coverImgView doCircleFrame];");
                sw2.WriteLine("             self.coverImgView.layer.cornerRadius = 2.0;");
                sw2.WriteLine("             self.coverImgView.layer.masksToBounds = YES;");
                sw2.WriteLine("             [self.contentView addSubview:self.coverImgView];");
                sw2.WriteLine("        }");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("        //标题");
                sw2.WriteLine("        if (!self.titleLabel) {");
                sw2.WriteLine("            self.titleLabel = [[UILabel alloc] initWithFrame:CGRectMake(CGRectGetMaxX(self.coverImgView.frame) + 10, 10, kScreen_Width-100, 20)];");
                sw2.WriteLine("            self.titleLabel.font = [UIFont systemFontOfSize:14];");
                sw2.WriteLine("            self.titleLabel.textColor = [UIColor colorWithHexString:@\"0x666666\"];");
                sw2.WriteLine("            [self.contentView addSubview:self.titleLabel];");
                sw2.WriteLine("        }");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("        //作者");
                sw2.WriteLine("         if (!self.authorLabel) {");
                sw2.WriteLine("            self.authorLabel = [[UILabel alloc] initWithFrame:CGRectMake(CGRectGetMaxX(self.coverImgView.frame) + 10, 40, kScreen_Width-100, 15)];");
                sw2.WriteLine("            self.authorLabel.font = [UIFont systemFontOfSize:12];");
                sw2.WriteLine("            self.authorLabel.textColor = [UIColor colorWithHexString:@\"0x999999\"];");
                sw2.WriteLine("            [self.contentView addSubview:self.authorLabel];");
                sw2.WriteLine("        }");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("        /* 样式2 图片宽屏上+文字下");
                sw2.WriteLine("        //图片");
                sw2.WriteLine("        if (!self.coverImgView) {");
                sw2.WriteLine("            self.coverImgView = [[UITapImageView alloc] initWithFrame:CGRectMake(0, 0, kScreen_Width, 150)];");
                sw2.WriteLine("            [self.contentView addSubview:self.coverImgView];");
                sw2.WriteLine("        }");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("        //标题");
                sw2.WriteLine("        if (!self.titleLabel) {");
                sw2.WriteLine("            self.titleLabel = [[UILabel alloc] initWithFrame:CGRectMake(10, 160, kScreen_Width-20, 30)];");
                sw2.WriteLine("            self.titleLabel.font = [UIFont systemFontOfSize:14];");
                sw2.WriteLine("            self.titleLabel.textColor = [UIColor colorWithHexString:@\"0x666666\"];");
                sw2.WriteLine("            [self.contentView addSubview:self.titleLabel];");
                sw2.WriteLine("        }*/");
                sw2.WriteLine("    }");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("    return self;");
                sw2.WriteLine("}");
                sw2.WriteLine("");
                sw2.WriteLine("//填充数据");
                sw2.WriteLine("- (void)setData:(" + strClassName + " *)data{");
                sw2.WriteLine("    _data = data;");
                sw2.WriteLine("    [self.coverImgView setImageWithUrl:[_data.cover_img urlImageWithCodePathResizeToView:_coverImgView] placeholderImage:kPlaceholderCodingSquareWidth(80.0) tapBlock:^(id obj) {}];");
                sw2.WriteLine("    self.titleLabel.text = _data." + strTableNameLower + "_title;");
                sw2.WriteLine("    self.authorLabel.text = @\"作者\";");
                sw2.WriteLine("}");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("@end");
                sw2.Close();

                //***DetailCell.h
                Directory.CreateDirectory(strFilePath);
                StreamWriter sw3 = new StreamWriter(strFilePath + "\\" + strClassName + "DetailCell.h", false, Encoding.GetEncoding("utf-8"));
                sw3.WriteLine("//");
                sw3.WriteLine("//  " + strClassName + "DetailCell.h");
                sw3.WriteLine("//  " + strProjectName);
                sw3.WriteLine("//");
                sw3.WriteLine("//  Created by " + strAuthor + " on " + DateTime.Now.ToString("yyyy/M/d") + ".");
                sw3.WriteLine("//  Copyright © " + DateTime.Now.Year + "年 " + strCompany + ". All rights reserved.");
                sw3.WriteLine("//");
                sw3.WriteLine("\r\n");
                sw3.WriteLine("#define kCellIdentifier_Detail @\"" + strClassName + "DetailCell\"");
                sw3.WriteLine("\r\n");
                sw3.WriteLine("#import <UIKit/UIKit.h>");
                sw3.WriteLine("#import \"" + strClassName + ".h\"");
                sw3.WriteLine("\r\n");
                sw3.WriteLine("@interface " + strClassName + "DetailCell : UITableViewCell <UIWebViewDelegate>");
                sw3.WriteLine("\r\n");
                sw3.WriteLine("@property (strong, nonatomic) " + strClassName + " *data;");
                sw3.WriteLine("@property (nonatomic, copy) void (^cellRefreshBlock) ();");
                sw3.WriteLine("@property (nonatomic, copy) void (^cellHeightChangedBlock) ();");
                sw3.WriteLine("@property (nonatomic, copy) void (^loadRequestBlock)(NSURLRequest *curRequest);");
                sw3.WriteLine("\r\n");
                sw3.WriteLine("+ (CGFloat)cellHeightWithObj:(id)obj;");
                sw3.WriteLine("\r\n");
                sw3.WriteLine("@end");
                sw3.Close();

                //***DetailCell.m
                Directory.CreateDirectory(strFilePath);
                StreamWriter sw4 = new StreamWriter(strFilePath + "\\" + strClassName + "DetailCell.m", false, Encoding.GetEncoding("utf-8"));
                sw4.WriteLine("//");
                sw4.WriteLine("//  " + strClassName + "DetailCell.m");
                sw4.WriteLine("//  " + strProjectName);
                sw4.WriteLine("//");
                sw4.WriteLine("//  Created by " + strAuthor + " on " + DateTime.Now.ToString("yyyy/M/d") + ".");
                sw4.WriteLine("//  Copyright © " + DateTime.Now.Year + "年 " + strCompany + ". All rights reserved.");
                sw4.WriteLine("//");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("#define kTweetDetailCell_PadingLeft 60.0");
                sw4.WriteLine("#define kTweet_TimtFont [UIFont systemFontOfSize:12]");
                sw4.WriteLine("#define kTweetDetailCell_LikeComment_Height 27.0");
                sw4.WriteLine("#define kTweetDetailCell_LikeComment_Width 50.0");
                sw4.WriteLine("#define kTweetDetailCell_ContentWidth (kScreen_Width - 2*kPaddingLeftWidth)");
                sw4.WriteLine("#define kTweetDetailCell_PadingTop 200.0");
                sw4.WriteLine("#define kTweetDetailCell_PadingBottom 10.0");
                sw4.WriteLine("#define kTweetDetailCell_LikeUserCCell_Height 25.0");
                sw4.WriteLine("#define kTweetDetailCell_LikeUserCCell_Pading 10.0");
                sw4.WriteLine("#define kTweetDetailCell_MaxCollectionNum (kDevice_Is_iPhone6Plus? 12: kDevice_Is_iPhone6? 11: 9)");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("#import \"" + strClassName + "DetailCell.h\"");
                sw4.WriteLine("#import \"UICustomCollectionView.h\"");
                sw4.WriteLine("#import \"Coding_NetAPIManager.h\"");
                sw4.WriteLine("#import \"WebContentManager.h\"");
                sw4.WriteLine("#import \"CodingShareView.h\"");
                sw4.WriteLine("#import \"SendRewardManager.h\"");
                sw4.WriteLine("");
                sw4.WriteLine("@interface " + strClassName + "DetailCell ()");
                sw4.WriteLine("");
                sw4.WriteLine("@property (strong, nonatomic) UITapImageView *coverImgView;");
                sw4.WriteLine("@property (strong, nonatomic) UILabel *titleLabel, *timeLabel, *addressLabel, *peopleLabel, *lineLabel, *authorLabel, *dateLabel, *visitLabel;");
                sw4.WriteLine("@property (strong, nonatomic) UIImageView *timeIconView, *addressIconView, *peopleIconView;");
                sw4.WriteLine("@property (strong, nonatomic) UIWebView *webContentView;");
                sw4.WriteLine("@property (strong, nonatomic) UIActivityIndicatorView *activityIndicator;");
                sw4.WriteLine("");
                sw4.WriteLine("@end");
                sw4.WriteLine("@implementation " + strClassName + "DetailCell");
                sw4.WriteLine("");
                sw4.WriteLine("- (id)initWithStyle:(UITableViewCellStyle)style reuseIdentifier:(NSString *)reuseIdentifier");
                sw4.WriteLine("{");
                sw4.WriteLine("    self = [super initWithStyle:style reuseIdentifier:reuseIdentifier];");
                sw4.WriteLine("    if (self) {");
                sw4.WriteLine("        // Initialization code");
                sw4.WriteLine("        self.selectionStyle = UITableViewCellSelectionStyleNone;");
                sw4.WriteLine("        self.backgroundColor = [UIColor clearColor];");
                sw4.WriteLine("        if (!self.coverImgView) {");
                sw4.WriteLine("            self.coverImgView = [[UITapImageView alloc] initWithFrame:CGRectMake(0, 0, kScreen_Width, 150)];");
                sw4.WriteLine("            [self.contentView addSubview:self.coverImgView];");
                sw4.WriteLine("        }");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("        if (!self.titleLabel) {");
                sw4.WriteLine("            self.titleLabel = [[UILabel alloc] initWithFrame:CGRectMake(10, 170, kScreen_Width - 10, 25)];");
                sw4.WriteLine("            self.titleLabel.font = [UIFont boldSystemFontOfSize: 17.0];");
                sw4.WriteLine("            self.titleLabel.textColor = [UIColor colorWithHexString:@\"0x333333\"];");
                sw4.WriteLine("            [self.contentView addSubview:self.titleLabel];");
                sw4.WriteLine("        }");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("        //分割线");
                sw4.WriteLine("        if (!self.authorLabel) {");
                sw4.WriteLine("            self.authorLabel = [[UILabel alloc] initWithFrame:CGRectMake(12, 210, 100, 20)];");
                sw4.WriteLine("            self.authorLabel.font = kTweet_TimtFont;");
                sw4.WriteLine("            self.authorLabel.textColor = [UIColor colorWithHexString:@\"0x999999\"];");
                sw4.WriteLine("            [self.contentView addSubview:self.authorLabel];");
                sw4.WriteLine("        }");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("        //发布日期");
                sw4.WriteLine("        if (!self.dateLabel) {");
                sw4.WriteLine("            self.dateLabel = [[UILabel alloc] initWithFrame:CGRectMake(120, 210, 100, 20)];");
                sw4.WriteLine("            self.dateLabel.font = kTweet_TimtFont;");
                sw4.WriteLine("            self.dateLabel.textColor = [UIColor colorWithHexString:@\"0x999999\"];");
                sw4.WriteLine("            [self.contentView addSubview:self.dateLabel];");
                sw4.WriteLine("        }");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("        //访问量");
                sw4.WriteLine("        if (!self.visitLabel) {");
                sw4.WriteLine("            self.visitLabel = [[UILabel alloc] initWithFrame:CGRectMake(kScreen_Width - 60, 210, 60, 20)];");
                sw4.WriteLine("            self.visitLabel.font = kTweet_TimtFont;");
                sw4.WriteLine("            self.visitLabel.textColor = [UIColor colorWithHexString:@\"0x999999\"];");
                sw4.WriteLine("            [self.contentView addSubview:self.visitLabel];");
                sw4.WriteLine("        }");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("        //时间");
                sw4.WriteLine("        if (!self.timeIconView) {");
                sw4.WriteLine("            self.timeIconView = [[UIImageView alloc] initWithFrame:CGRectMake(kPaddingLeftWidth, 240+2, 14, 14)];");
                sw4.WriteLine("            self.timeIconView.image = [UIImage imageNamed:@\"ic_time\"];");
                sw4.WriteLine("            [self.contentView addSubview:self.timeIconView];");
                sw4.WriteLine("        }");
                sw4.WriteLine("        if (!self.timeLabel) {");
                sw4.WriteLine("            self.timeLabel = [[UILabel alloc] initWithFrame:CGRectMake(CGRectGetMaxX(self.timeIconView.frame) + 5, 240, kScreen_Width-kPaddingLeftWidth, 20)];");
                sw4.WriteLine("            self.timeLabel.font = kTweet_TimtFont;");
                sw4.WriteLine("            self.timeLabel.textColor = [UIColor colorWithHexString:@\"0x999999\"];");
                sw4.WriteLine("            [self.contentView addSubview:self.timeLabel];");
                sw4.WriteLine("        }");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("        //地址");
                sw4.WriteLine("        if (!self.addressIconView) {");
                sw4.WriteLine("            self.addressIconView = [[UIImageView alloc] initWithFrame:CGRectMake(kPaddingLeftWidth, 240+25+2, 14, 14)];");
                sw4.WriteLine("            self.addressIconView.image = [UIImage imageNamed:@\"ic_add\"];");
                sw4.WriteLine("            [self.contentView addSubview:self.addressIconView];");
                sw4.WriteLine("        }");
                sw4.WriteLine("        if (!self.addressLabel) {");
                sw4.WriteLine("            self.addressLabel = [[UILabel alloc] initWithFrame:CGRectMake(CGRectGetMaxX(self.addressIconView.frame) + 5, 240+25, kScreen_Width-kPaddingLeftWidth, 20)];");
                sw4.WriteLine("            self.addressLabel.font = kTweet_TimtFont;");
                sw4.WriteLine("            self.addressLabel.textColor = [UIColor colorWithHexString:@\"0x24d296\"];");
                sw4.WriteLine("            [self.contentView addSubview:self.addressLabel];");
                sw4.WriteLine("        }");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("        //人数");
                sw4.WriteLine("        if (!self.peopleIconView) {");
                sw4.WriteLine("            self.peopleIconView = [[UIImageView alloc] initWithFrame:CGRectMake(kPaddingLeftWidth,  240+25+25+2, 14, 14)];");
                sw4.WriteLine("            self.peopleIconView.image = [UIImage imageNamed:@\"ic_man\"];");
                sw4.WriteLine("            [self.contentView addSubview:self.peopleIconView];");
                sw4.WriteLine("        }");
                sw4.WriteLine("        if (!self.peopleLabel) {");
                sw4.WriteLine("            self.peopleLabel = [[UILabel alloc] initWithFrame:CGRectMake(CGRectGetMaxX(self.peopleIconView.frame) + 5, 240+25+25, kScreen_Width-kPaddingLeftWidth, 20)];");
                sw4.WriteLine("            self.peopleLabel.font = kTweet_TimtFont;");
                sw4.WriteLine("            self.peopleLabel.textColor = [UIColor colorWithHexString:@\"0x999999\"];");
                sw4.WriteLine("            [self.contentView addSubview:self.peopleLabel];");
                sw4.WriteLine("        }");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("        if (!self.webContentView) {");
                sw4.WriteLine("            self.webContentView = [[UIWebView alloc] initWithFrame:CGRectMake(kPaddingLeftWidth, 320, kTweetDetailCell_ContentWidth, 1)];");
                sw4.WriteLine("            self.webContentView.delegate = self;");
                sw4.WriteLine("            self.webContentView.scrollView.scrollEnabled = NO;");
                sw4.WriteLine("            self.webContentView.scrollView.scrollsToTop = NO;");
                sw4.WriteLine("            self.webContentView.scrollView.bounces = NO;");
                sw4.WriteLine("            self.webContentView.backgroundColor = [UIColor clearColor];");
                sw4.WriteLine("            self.webContentView.opaque = NO;");
                sw4.WriteLine("            [self.contentView addSubview:self.webContentView];");
                sw4.WriteLine("        }");
                sw4.WriteLine("        if (!_activityIndicator) {");
                sw4.WriteLine("            _activityIndicator = [[UIActivityIndicatorView alloc]initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleGray];");
                sw4.WriteLine("            _activityIndicator.hidesWhenStopped = YES;");
                sw4.WriteLine("            [_activityIndicator setCenter:CGPointMake(CGRectGetMidX(self.webContentView.frame), kTweetDetailCell_PadingTop+CGRectGetHeight(_activityIndicator.bounds)/2)];");
                sw4.WriteLine("            [self.contentView addSubview:_activityIndicator];");
                sw4.WriteLine("        }");
                sw4.WriteLine("    }");
                sw4.WriteLine("    return self;");
                sw4.WriteLine("}");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("- (void)setData:(" + strClassName + " *)data{");
                sw4.WriteLine("    _data = data;");
                sw4.WriteLine("    [self.coverImgView setImageWithUrl:[_data.cover_img urlImageWithCodePathResizeToView:_coverImgView] placeholderImage:kPlaceholderMonkeyRoundView(_coverImgView) tapBlock:^(id obj) {}];");
                sw4.WriteLine("    self.titleLabel.text = _data." + strTableNameLower + "_title;");
                sw4.WriteLine("    self.dateLabel.text = [_data.created_at stringDisplay_HHmm];//NSDate");
                sw4.WriteLine("//    if ([_data.created_at doubleValue] > 0) {");
                sw4.WriteLine("//        //NSNumber");
                sw4.WriteLine("//        NSDate *date =[NSDate dateWithTimeIntervalSince1970: ((double)(_data.created_at.longLongValue))/1000.0];");
                sw4.WriteLine("//        if (date) {");
                sw4.WriteLine("//            self.dateLabel.text = [date stringWithFormat:@\"yyyy年MM月dd日 HH: mm\"];");
                sw4.WriteLine("//        }");
                sw4.WriteLine("//    }");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("    self.visitLabel.text = [NSString stringWithFormat:@\"阅读 %@\",_data." + strTableNameLower + "_id];//需要修改");
                sw4.WriteLine("    self.timeLabel.text =  [NSString stringWithFormat:@\"时间：%@\",_data." + strTableNameLower + "_id];//需要修改");
                sw4.WriteLine("    self.addressLabel.text = [NSString stringWithFormat:@\"地址：%@\",_data." + strTableNameLower + "_id];//需要修改");
                sw4.WriteLine("    self.peopleLabel.text = [NSString stringWithFormat:@\"人数：%@\",_data." + strTableNameLower + "_id];//需要修改");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("    //内容");
                sw4.WriteLine("    [self.webContentView setHeight:_data.contentHeight];");
                sw4.WriteLine("    if (!_webContentView.isLoading) {");
                sw4.WriteLine("        [_activityIndicator startAnimating];");
                sw4.WriteLine("        if (_data.htmlMedia.contentOrigional) {");
                sw4.WriteLine("            [self.webContentView loadHTMLString:[WebContentManager bubblePatternedWithContent:_data.htmlMedia.contentOrigional] baseURL:nil];");
                sw4.WriteLine("        }");
                sw4.WriteLine("    }");
                sw4.WriteLine("}");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("#pragma mark cell Height");
                sw4.WriteLine("//注意修改参数");
                sw4.WriteLine("+ (CGFloat)cellHeightWithObj:(id)obj{");
                sw4.WriteLine("    CGFloat cellHeight = 0;");
                sw4.WriteLine("    if (obj && [obj isKindOfClass:[" + strClassName + " class]]) {");
                sw4.WriteLine("        " + strClassName + " *data = (" + strClassName + " *)obj;");
                sw4.WriteLine("        cellHeight += kTweetDetailCell_PadingTop;");
                sw4.WriteLine("        cellHeight += [[self class] contentHeightWithData:data];");
                sw4.WriteLine("        cellHeight += 10;");
                sw4.WriteLine("        cellHeight += 5 + kTweetDetailCell_LikeComment_Height;");
                sw4.WriteLine("        cellHeight += kTweetDetailCell_PadingBottom;");
                sw4.WriteLine("    }");
                sw4.WriteLine("    return cellHeight;");
                sw4.WriteLine("}");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("+ (CGFloat)contentHeightWithData:(" + strClassName + " *)data{");
                sw4.WriteLine("    return data.contentHeight;");
                sw4.WriteLine("}");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("#pragma mark UIWebViewDelegate");
                sw4.WriteLine("- (BOOL)webView:(UIWebView *)webView shouldStartLoadWithRequest:(NSURLRequest *)request navigationType:(UIWebViewNavigationType)navigationType{");
                sw4.WriteLine("    NSString *strLink = request.URL.absoluteString;");
                sw4.WriteLine("    DebugLog(@\"strLink =[%@]\",strLink);");
                sw4.WriteLine("    if ([strLink rangeOfString:@\"about:blank\"].location != NSNotFound) {");
                sw4.WriteLine("        return YES;");
                sw4.WriteLine("    }else{");
                sw4.WriteLine("        if (_loadRequestBlock) {");
                sw4.WriteLine("            _loadRequestBlock(request);");
                sw4.WriteLine("        }");
                sw4.WriteLine("        return NO;");
                sw4.WriteLine("    }");
                sw4.WriteLine("}");
                sw4.WriteLine("- (void)webViewDidStartLoad:(UIWebView *)webView{");
                sw4.WriteLine("    [_activityIndicator startAnimating];");
                sw4.WriteLine("}");
                sw4.WriteLine("- (void)webViewDidFinishLoad:(UIWebView *)webView{");
                sw4.WriteLine("    [self refreshwebContentView];");
                sw4.WriteLine("    [_activityIndicator stopAnimating];");
                sw4.WriteLine("    CGFloat scrollHeight = webView.scrollView.contentSize.height;");
                sw4.WriteLine("    if (ABS(scrollHeight - _data.contentHeight) > 5) {");
                sw4.WriteLine("        webView.scalesPageToFit = YES;");
                sw4.WriteLine("        _data.contentHeight = scrollHeight;");
                sw4.WriteLine("        if (_cellHeightChangedBlock) {");
                sw4.WriteLine("            _cellHeightChangedBlock();");
                sw4.WriteLine("        }");
                sw4.WriteLine("    }");
                sw4.WriteLine("}");
                sw4.WriteLine("- (void)webView:(UIWebView *)webView didFailLoadWithError:(NSError *)error{");
                sw4.WriteLine("    [_activityIndicator stopAnimating];");
                sw4.WriteLine("    if([error code] == NSURLErrorCancelled)");
                sw4.WriteLine("        return;");
                sw4.WriteLine("    else");
                sw4.WriteLine("        DebugLog(@\"%@\", error.description);");
                sw4.WriteLine("}");
                sw4.WriteLine("- (void)refreshwebContentView{");
                sw4.WriteLine("    if (_webContentView) {");
                sw4.WriteLine("        //修改服务器页面的meta的值");
                sw4.WriteLine("        NSString *meta = [NSString stringWithFormat:@\"document.getElementsByName(\\\"viewport\\\")[0].content = \\\"width=%f, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no\\\"\", CGRectGetWidth(_webContentView.frame)]; ");
                sw4.WriteLine("        [_webContentView stringByEvaluatingJavaScriptFromString:meta];");
                sw4.WriteLine("    }");
                sw4.WriteLine("}");
                sw4.WriteLine("\r\n");
                sw4.WriteLine("@end");
                sw4.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ModelHelper), ex, "生成iOS客户端 Views", "CreateiOSViewsClass",false);
            }
        }

        /// <summary>
        /// 生成iOS客户端 Controllers
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strProjectName"></param>
        /// <param name="strTableName"></param>
        /// <param name="strPackage"></param>
        public static void CreateControllersClass(string strFilePath, string strProjectName, string strTableName, string strPackage)
        {
            try
            {
                //读取版权信息
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");
                string strCompany = iniFile.GetString("COPYRIGHT", "COMPANY", "");
                string strAuthor = iniFile.GetString("COPYRIGHT", "AUTHOR", "");
                string strVersion = iniFile.GetString("COPYRIGHT", "VERSION", "");
                string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");
                string strCreateDate = iniFile.GetString("BASE", "CREATE_DATE", "");

                string strClassName = CommonHelper.GetClassName(strTableName);//类名
                strClassName = CommonHelper.GetTableNameUpper(strClassName);//如：News,NewsType
                string strTableNameLower = CommonHelper.GetTableNameFirtWordLower(strTableName);//如：news,newstype

                Directory.CreateDirectory(strFilePath);

                //***ViewController.h
                StreamWriter sw = new StreamWriter(strFilePath + "\\" + strClassName + "ViewController.h", false, Encoding.GetEncoding("utf-8"));
                sw.WriteLine("//");
                sw.WriteLine("//  " + strClassName + "ViewController.h");
                sw.WriteLine("//  " + strProjectName);
                sw.WriteLine("//");
                sw.WriteLine("//  Created by " + strAuthor + " on " + DateTime.Now.ToString("yyyy/M/d") + ".");
                sw.WriteLine("//  Copyright © " + DateTime.Now.Year + "年 " + strCompany + ". All rights reserved.");
                sw.WriteLine("//");
                sw.WriteLine("\r\n");
                sw.WriteLine("#import \"BaseViewController.h\"");
                sw.WriteLine("#import \"ODRefreshControl.h\"");
                sw.WriteLine("#import \"" + strClassName + "Model.h\"");
                sw.WriteLine("\r\n");
                sw.WriteLine("@interface " + strClassName + "ViewController : BaseViewController<UITableViewDataSource, UITableViewDelegate>");
                sw.WriteLine("\r\n");
                sw.WriteLine("@end");
                sw.Close();

                //***ViewController.m
                //读取模板内容
                FileStream fsList = new FileStream(Application.StartupPath + "\\template\\ios\\list.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader srList = new StreamReader(fsList, Encoding.UTF8);
                string strGetPageHTMLContentForListPage = srList.ReadToEnd();
                srList.Close();
                fsList.Close();

                strGetPageHTMLContentForListPage = ReplaceHTMLDataForiOSList(strGetPageHTMLContentForListPage, strTableName, strProjectName);
                StreamWriter swList = new StreamWriter(strFilePath + "\\" + strClassName + "ViewController.m", false, Encoding.GetEncoding("utf-8"));
                swList.WriteLine(strGetPageHTMLContentForListPage);
                swList.Close();


                //***DetailsViewController.h
                StreamWriter sw2 = new StreamWriter(strFilePath + "\\" + strClassName + "DetailsViewController.h", false, Encoding.GetEncoding("utf-8"));
                sw2.WriteLine("//");
                sw2.WriteLine("//  " + strClassName + "DetailsViewController.h");
                sw2.WriteLine("//  " + strProjectName);
                sw2.WriteLine("//");
                sw2.WriteLine("//  Created by " + strAuthor + " on " + DateTime.Now.ToString("yyyy/M/d") + ".");
                sw2.WriteLine("//  Copyright © " + DateTime.Now.Year + "年 " + strCompany + ". All rights reserved.");
                sw2.WriteLine("//");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("#import \"BaseViewController.h\"");
                sw2.WriteLine("#import \"" + strClassName + "Model.h\"");
                sw2.WriteLine("#import \"ODRefreshControl.h\"");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("@interface " + strClassName + "DetailsViewController : BaseViewController<UITableViewDataSource, UITableViewDelegate, TTTAttributedLabelDelegate>");
                sw2.WriteLine("@property (strong, nonatomic) " + strClassName + " *curData;");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("- (void)refreshData;");
                sw2.WriteLine("\r\n");
                sw2.WriteLine("@end");
                sw2.Close();


                //***DetailsViewController.m
                //读取模板内容
                FileStream fsDetail = new FileStream(Application.StartupPath + "\\template\\ios\\detail.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader srDetail = new StreamReader(fsDetail, Encoding.UTF8);
                string strGetPageHTMLContentForDetailPage = srDetail.ReadToEnd();
                srDetail.Close();
                fsDetail.Close();

                strGetPageHTMLContentForDetailPage = ReplaceHTMLDataForiOSDetail(strGetPageHTMLContentForDetailPage, strTableName, strProjectName);
                StreamWriter swDetail = new StreamWriter(strFilePath + "\\" + strClassName + "DetailsViewController.m", false, Encoding.GetEncoding("utf-8"));
                swDetail.WriteLine(strGetPageHTMLContentForDetailPage);
                swDetail.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ModelHelper), ex, "生成iOS客户端 Controllers", "CreateiOSControllersClass",false);
            }
        }


        /// <summary>
        /// 获取iOS端定义参数列表
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetDefineParaListForiOS(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                int nNum = 0;
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT,DATA_TYPE FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnNameUpper = CommonHelper.GetTableNameUpper(strColumnName);//如：UserName
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//注释
                    strColumnComment = CommonHelper.GetColumnKeyComment(strColumnComment);

                    string strDataType = dr["DATA_TYPE"].ToString();//数据类型
                    strDataType = CodeRobot.Utility.StringHelper.GetCSharpDBType(strDataType);

                    //获取数据类型
                    string strValue = "";
                    if (strDataType == "int")
                    {
                        strValue = "int";
                    }
                    else if (strDataType == "float")
                    {
                        strValue = "int";
                    }
                    else if (strDataType == "double")
                    {
                        strValue = "int";
                    }
                    else if (strDataType == "decimal")
                    {
                        strValue = "int";
                    }
                    else if (strDataType == "bool")
                    {
                        strValue = "int";
                    }
                    else if (strDataType == "DateTime")
                    {
                        strValue = "long";
                    }
                    else if (strDataType == "string")
                    {
                        strValue = "String";
                    }
                    else
                    {
                        strValue = "String";
                    }

                    string strColumn = "";
                    if (strValue == "int")
                    {
                        strColumn = "@property (strong, nonatomic) NSNumber *" + strColumnName + ";";
                    }
                    else if (strValue == "long")
                    {
                        strColumn = "@property (strong, nonatomic) NSDate *" + strColumnName + ";";
                    }
                    else
                    {
                        //此处是iOS客户端需要优化的地方，内容必须定义为content，如果修复此BUG，则可以删除此段代码
                        if (strColumnName.Contains("content"))
                        {
                            strColumnName = "content";
                        }
                        strColumn = "@property (strong, nonatomic) NSString *" + strColumnName + ";";
                    }

                    strReturnValue += "//" + strColumnComment + "\r\n" + strColumn + "\r\n";
                    nNum++;
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ModelHelper), ex, "获取iOS端定义参数列表", "GetDefineParaListForiOS",false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 获取iOS实体类缓存数据方法
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public static string GetSaveModelListForiOS(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                string strTableNameLower = CommonHelper.GetTableNameFirtWordLower(strTableName);//如：news,newstype

                int nNum = 0;
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT,DATA_TYPE FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnNameUpper = CommonHelper.GetTableNameUpper(strColumnName);//如：UserName
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//注释
                    strColumnComment = CommonHelper.GetColumnKeyComment(strColumnComment);

                    string strDataType = dr["DATA_TYPE"].ToString();//数据类型
                    strDataType = CodeRobot.Utility.StringHelper.GetCSharpDBType(strDataType);

                    string strColumn = strColumn = "    " + strTableNameLower + "." + strColumnName + " = [_" + strColumnName + " copy];";

                    strReturnValue += strColumn + "\r\n";
                    nNum++;
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ModelHelper), ex, "获取iOS端定义参数列表", "GetDefineParaListForiOS",false);
            }

            return strReturnValue;
        }


        /// <summary>
        /// 读取ViewController模板内容，并替换成对应的表数据
        /// </summary>
        /// <param name="strContent"></param>
        /// <param name="strTableName"></param>
        /// <param name="strProjectName"></param>
        /// <returns></returns>
        public static string ReplaceHTMLDataForiOSList(string strContent, string strTableName, string strProjectName)
        {
            string strReturnValue = "";

            try
            {
                //读取版权信息
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");
                string strCompany = iniFile.GetString("COPYRIGHT", "COMPANY", "");
                string strAuthor = iniFile.GetString("COPYRIGHT", "AUTHOR", "");
                string strVersion = iniFile.GetString("COPYRIGHT", "VERSION", "");
                string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");
                string strCreateDate = iniFile.GetString("BASE", "CREATE_DATE", "");

                string strClassName = CommonHelper.GetClassName(strTableName);//类名
                strClassName = CommonHelper.GetTableNameUpper(strClassName);//如：News,NewsType
                string strTableNameLower = CommonHelper.GetTableNameFirtWordLower(strTableName);//如：news,newstype

                //替换文件名-1
                strContent = strContent.Replace("{classname}", strClassName);
                strContent = strContent.Replace("{projectname}", strProjectName);
                strContent = strContent.Replace("{classnamemodel}", strClassName + "Model");
                strContent = strContent.Replace("{tablenamelower}", strTableNameLower);
                strContent = strContent.Replace("{author}", strAuthor);
                strContent = strContent.Replace("{date}", DateTime.Now.ToString("yyyy/M/d"));
                strContent = strContent.Replace("{year}", DateTime.Now.Year.ToString());
                strContent = strContent.Replace("{company}", strCompany);

                strReturnValue = strContent;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "处理HTML代码", "ReplaceHTMLDataForiOSList",false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 读取DetailViewController模板内容，并替换成对应的表数据
        /// </summary>
        /// <param name="strContent"></param>
        /// <param name="strTableName"></param>
        /// <param name="strProjectName"></param>
        /// <returns></returns>
        public static string ReplaceHTMLDataForiOSDetail(string strContent, string strTableName, string strProjectName)
        {
            string strReturnValue = "";

            try
            {
                //读取版权信息
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");
                string strCompany = iniFile.GetString("COPYRIGHT", "COMPANY", "");
                string strAuthor = iniFile.GetString("COPYRIGHT", "AUTHOR", "");
                string strVersion = iniFile.GetString("COPYRIGHT", "VERSION", "");
                string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");
                string strCreateDate = iniFile.GetString("BASE", "CREATE_DATE", "");

                //获取主键ID
                string strPrimaryID = CommonHelper.GetPrimaryKey(strTableName);
                string strClassName = CommonHelper.GetClassName(strTableName);//类名
                strClassName = CommonHelper.GetTableNameUpper(strClassName);//如：News,NewsType
                string strTableNameLower = CommonHelper.GetTableNameFirtWordLower(strTableName);//如：news,newstype

                //替换文件名-1
                strContent = strContent.Replace("{classname}", strClassName);
                strContent = strContent.Replace("{projectname}", strProjectName);
                strContent = strContent.Replace("{classnamemodel}", strClassName + "Model");
                strContent = strContent.Replace("{tablenamelower}", strTableNameLower);
                strContent = strContent.Replace("{author}", strAuthor);
                strContent = strContent.Replace("{date}", DateTime.Now.ToString("yyyy/M/d"));
                strContent = strContent.Replace("{year}", DateTime.Now.Year.ToString());
                strContent = strContent.Replace("{company}", strCompany);
                strContent = strContent.Replace("{primaryid}", strPrimaryID);

                strReturnValue = strContent;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "处理HTML代码", "ReplaceHTMLDataForiOSDetail",false);
            }

            return strReturnValue;
        }

    }
}
